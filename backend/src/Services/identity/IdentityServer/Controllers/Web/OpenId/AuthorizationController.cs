using IdentityServer.Controllers.Web.Base;
using IdentityServer.Models.Authorization;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using OpenIddict.Validation.AspNetCore;
using System.Collections.Immutable;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace IdentityServer.Controllers.Web.OpenId
{
    public class AuthorizationController : BaseWebController<AuthorizationController>
    {
        private readonly IOpenIddictApplicationManager _applicationManager;
        private readonly IOpenIddictAuthorizationManager _authorizationManager;
        private readonly IOpenIddictScopeManager _scopeManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        readonly RoleManager<AppRole> _roleManager;
        private readonly IAntiforgery _antiforgery;
        private readonly IOpenIddictTokenManager _tokenManager;

        public AuthorizationController(
            IOpenIddictApplicationManager applicationManager,
            IOpenIddictAuthorizationManager authorizationManager,
            IOpenIddictScopeManager scopeManager,
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            IAntiforgery antiforgery,
            IOpenIddictTokenManager tokenManager)
        {
            _applicationManager = applicationManager;
            _authorizationManager = authorizationManager;
            _scopeManager = scopeManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _antiforgery = antiforgery;
            _tokenManager = tokenManager;
        }

        [HttpGet("~/connect/authorize")]
        [HttpPost("~/connect/authorize")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Authorize()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            // If prompt=login was specified by the client application,
            // immediately return the user agent to the login page.
            if (request.HasPrompt(Prompts.Login))
            {
                // To avoid endless login -> authorization redirects, the prompt=login flag
                // is removed from the authorization request payload before redirecting the user.
                var prompt = string.Join(" ", request.GetPrompts().Remove(Prompts.Login));

                var parameters = Request.HasFormContentType ?
                    Request.Form.Where(parameter => parameter.Key != Parameters.Prompt).ToList() :
                    Request.Query.Where(parameter => parameter.Key != Parameters.Prompt).ToList();

                parameters.Add(KeyValuePair.Create(Parameters.Prompt, new StringValues(prompt)));

                return Challenge(
                    authenticationSchemes: IdentityConstants.ApplicationScheme,
                    properties: new AuthenticationProperties
                    {
                        RedirectUri = Request.PathBase + Request.Path + QueryString.Create(parameters)
                    });
            }

            // Retrieve the user principal stored in the authentication cookie.
            // If a max_age parameter was provided, ensure that the cookie is not too old.
            // If the user principal can't be extracted or the cookie is too old, redirect the user to the login page.
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
            if (result == null || !result.Succeeded || (request.MaxAge != null && result.Properties?.IssuedUtc != null &&
                DateTimeOffset.UtcNow - result.Properties.IssuedUtc > TimeSpan.FromSeconds(request.MaxAge.Value)))
            {
                // If the client application requested promptless authentication,
                // return an error indicating that the user is not logged in.
                if (request.HasPrompt(Prompts.None))
                {
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string?>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.LoginRequired,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is not logged in."
                        }));
                }

                return Challenge(
                    authenticationSchemes: IdentityConstants.ApplicationScheme,
                    properties: new AuthenticationProperties
                    {
                        RedirectUri = Request.PathBase + Request.Path + QueryString.Create(
                            Request.HasFormContentType ? Request.Form.ToList() : Request.Query.ToList())
                    });
            }

            // Retrieve the profile of the logged in user.
            var user = await _userManager.GetUserAsync(result.Principal) ??
                throw new InvalidOperationException("The user details cannot be retrieved.");

            // Retrieve the application details from the database.
            var application = await _applicationManager.FindByClientIdAsync(request.ClientId!) ??
                throw new InvalidOperationException("Details concerning the calling client application cannot be found.");

            // Retrieve the permanent authorizations associated with the user and the calling client application.
            var client = await _applicationManager.GetIdAsync(application);
            var authorizations = await _authorizationManager.FindAsync(
                subject: await _userManager.GetUserIdAsync(user),
                client: client!,// await _applicationManager.GetIdAsync(application),
                status: Statuses.Valid,
                type: AuthorizationTypes.Permanent,
                scopes: request.GetScopes()).ToListAsync();

            switch (await _applicationManager.GetConsentTypeAsync(application))
            {
                // If the consent is external (e.g when authorizations are granted by a sysadmin),
                // immediately return an error if no authorization can be found in the database.
                case ConsentTypes.External when !authorizations.Any():
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string?>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.ConsentRequired,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                                "The logged in user is not allowed to access this client application."
                        }));

                // If the consent is implicit or if an authorization was found,
                // return an authorization response without displaying the consent form.
                case ConsentTypes.Implicit:
                case ConsentTypes.External when authorizations.Any():
                case ConsentTypes.Explicit when authorizations.Any() && !request.HasPrompt(Prompts.Consent):
                    var principal = await _signInManager.CreateUserPrincipalAsync(user);

                    var roles = await _userManager.GetRolesAsync(user);
                    var roledictionary = new Dictionary<string, object>();
                    foreach (var role in roles)
                    {
                        var permissions = new List<string>();
                        var approle = await _roleManager.FindByNameAsync(role);

                        var allClaims = await _roleManager.GetClaimsAsync(approle!);
                        foreach (var claim in allClaims)
                        {
                            permissions.Add($"{claim.Value}");
                        }
                        roledictionary.Add(role, permissions);
                    }
                    var rolestring = JsonSerializer.Serialize(roledictionary);
                    principal.SetClaim("RolePermission", rolestring);

                    // Note: in this sample, the granted scopes match the requested scope
                    // but you may want to allow the user to uncheck specific scopes.
                    // For that, simply restrict the list of scopes before calling SetScopes.
                    principal.SetScopes(request.GetScopes());
                    principal.SetResources(await _scopeManager.ListResourcesAsync(principal.GetScopes()).ToListAsync());

                    // Automatically create a permanent authorization to avoid requiring explicit consent
                    // for future authorization or token requests containing the same scopes.
                    var authorization = authorizations.LastOrDefault();
                    authorization ??= await _authorizationManager.CreateAsync(
                            principal: principal,
                            subject: await _userManager.GetUserIdAsync(user),
                            client: await _applicationManager.GetIdAsync(application) ?? "",
                            type: AuthorizationTypes.Permanent,
                            scopes: principal.GetScopes());

                    principal.SetAuthorizationId(await _authorizationManager.GetIdAsync(authorization));

                    foreach (var claim in principal.Claims)
                    {
                        claim.SetDestinations(GetDestinations(claim));
                    }

                    return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                // At this point, no authorization was found in the database and an error must be returned
                // if the client application specified prompt=none in the authorization request.
                case ConsentTypes.Explicit when request.HasPrompt(Prompts.None):
                case ConsentTypes.Systematic when request.HasPrompt(Prompts.None):
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string?>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.ConsentRequired,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                                "Interactive user consent is required."
                        }));

                // In every other case, render the consent form.
                default:
                    var useconsent = true; // _configuration.GetValue<bool>("UseConsent");
                    if (useconsent)
                    {
                        return View(new AuthorizeViewModel
                        {
                            ApplicationName = await _applicationManager.GetDisplayNameAsync(application),
                            Scope = request.Scope
                        });
                    }
                    else
                    {
                        return await Accept();
                    }
            }
        }

        [Authorize, FormValueRequired("submit.Accept")]
        [HttpPost("~/connect/authorize"), ValidateAntiForgeryToken]
        [EnableCors("CorsPolicy")]
        public async Task<IActionResult> Accept()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            // Retrieve the profile of the logged in user.
            var user = await _userManager.GetUserAsync(User) ??
                throw new InvalidOperationException("The user details cannot be retrieved.");

            // Retrieve the application details from the database.
            var application = await _applicationManager.FindByClientIdAsync(request.ClientId!) ??
                throw new InvalidOperationException("Details concerning the calling client application cannot be found.");

            // Retrieve the permanent authorizations associated with the user and the calling client application.
            var authorizations = await _authorizationManager.FindAsync(
                subject: await _userManager.GetUserIdAsync(user),
                client: await _applicationManager.GetIdAsync(application!) ?? "",
                status: Statuses.Valid,
                type: AuthorizationTypes.Permanent,
                scopes: request.GetScopes()).ToListAsync();

            // Note: the same check is already made in the other action but is repeated
            // here to ensure a malicious user can't abuse this POST-only endpoint and
            // force it to return a valid response without the external authorization.
            if (!authorizations.Any() && await _applicationManager.HasConsentTypeAsync(application, ConsentTypes.External))
            {
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.ConsentRequired,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "The logged in user is not allowed to access this client application."
                    }));
            }

            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            var roledictionary = new Dictionary<string, object>();
            foreach (var role in roles)
            {
                var permissions = new List<string>();
                var approle = await _roleManager.FindByNameAsync(role);

                var allClaims = await _roleManager.GetClaimsAsync(approle!);
                foreach (var claim in allClaims)
                {
                    permissions.Add($"{claim.Value}");
                }
                roledictionary.Add(role, permissions);
            }
            var rolestring = JsonSerializer.Serialize(roledictionary);
            principal.SetClaim("RolePermission", rolestring);

            // Note: in this sample, the granted scopes match the requested scope
            // but you may want to allow the user to uncheck specific scopes.
            // For that, simply restrict the list of scopes before calling SetScopes.
            principal.SetScopes(request.GetScopes());
            principal.SetResources(await _scopeManager.ListResourcesAsync(principal.GetScopes()).ToListAsync());

            // Automatically create a permanent authorization to avoid requiring explicit consent
            // for future authorization or token requests containing the same scopes.
            var authorization = authorizations.LastOrDefault();
            authorization ??= await _authorizationManager.CreateAsync(
                    principal: principal,
                    subject: await _userManager.GetUserIdAsync(user),
                    client: await _applicationManager.GetIdAsync(application) ?? "",
                    type: AuthorizationTypes.Permanent,
                    scopes: principal.GetScopes());

            principal.SetAuthorizationId(await _authorizationManager.GetIdAsync(authorization));

            foreach (var claim in principal.Claims)
            {
                claim.SetDestinations(GetDestinations(claim));
            }

            // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        [Authorize, FormValueRequired("submit.Deny")]
        [HttpPost("~/connect/authorize"), ValidateAntiForgeryToken]
        // Notify OpenIddict that the authorization grant has been denied by the resource owner
        // to redirect the user agent to the client application using the appropriate response_mode.
        public IActionResult Deny()
        {
            return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
        [HttpGet("~/connect/CsrfToken")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        public IActionResult CsrfToken(CancellationToken cancellationToken)
        {
            var tokens = _antiforgery.GetTokens(HttpContext);

            return Ok(new { csrfToken = tokens.RequestToken });
        }

        [HttpGet("~/connect/logout")]
        public IActionResult Logout()
        {
            return View();
        }

        [ActionName(nameof(Logout)), HttpPost("~/connect/logout")]
        public async Task<IActionResult> LogoutPost(CancellationToken cancellationToken = default)
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            // Ask ASP.NET Core Identity to delete the local and external cookies created
            // when the user agent is redirected from the external identity provider
            // after a successful authentication flow (e.g Google or Facebook).
            await _signInManager.SignOutAsync();

            // Ambil instance OpenIddictServerDispatcher
            //var dispatcher = HttpContext.RequestServices.GetRequiredService<OpenIddictServerDispatcher>();
            //var dispatcher = HttpContext.RequestServices.GetRequiredService<OpenIddictServerDispatcher>();
            var dd = await _tokenManager.FindBySubjectAsync("[userid]", cancellationToken).ToListAsync();
            var authresult = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            foreach (var token in await _tokenManager.FindBySubjectAsync("[userid]", cancellationToken).ToListAsync())
            {
                var rfd = await _tokenManager.TryRevokeAsync(token, cancellationToken);
            }
            //var accessToken = await HttpContext.GetTokenAsync("access_token");
            var accessToken = await HttpContext.GetTokenAsync("Authorization");
            if (!string.IsNullOrEmpty(accessToken))
            {
                //await dispatcher.DispatchAsync(new RevokeTokenRequest
                //{
                //    Token = accessToken
                //});
            }

            if (request.IsPasswordGrantType() || request.GrantType == "manual")
            {
                SignOut(authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                return Ok(new { message = "Sign out successful." });
            }

            // Returning a SignOutResult will ask OpenIddict to redirect the user agent
            // to the post_logout_redirect_uri specified by the client application or to
            // the RedirectUri specified in the authentication properties if none was set.
            return SignOut(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties
                {
                    RedirectUri = "/"
                });
        }

        [HttpPost("~/connect/token"), IgnoreAntiforgeryToken, Produces("application/json")]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            if (request.IsAuthorizationCodeGrantType() || request.IsRefreshTokenGrantType())
            {
                // Retrieve the claims principal stored in the authorization code/refresh token.
                var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                // Retrieve the user profile corresponding to the authorization code/refresh token.
                var user = await _userManager.FindByIdAsync(result.Principal?.GetClaim(Claims.Subject) ?? "");
                if (user is null)
                {
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string?>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The token is no longer valid."
                        }));
                }

                // Ensure the user is still allowed to sign in.
                if (!await _signInManager.CanSignInAsync(user))
                {
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string?>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is no longer allowed to sign in."
                        }));
                }

                var identity = new ClaimsIdentity(result.Principal?.Claims,
                    authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                    nameType: Claims.Name,
                    roleType: Claims.Role);

                if (identity.GetClaim(Claims.Picture) == null)
                {
                    identity.AddClaim(Claims.Picture, (user != null) ? user.ImageUrl ?? "" : "");
                }
                if (identity.GetClaim("first_name") == null)
                {
                    identity.AddClaim("first_name", (user != null) ? user.FirstName ?? "" : "");
                }
                if (identity.GetClaim("last_name") == null)
                {
                    identity.AddClaim("last_name", (user != null) ? user.LastName ?? "" : "");
                }
                if (identity.GetClaim("category_id") == null)
                {
                    identity.AddClaim("category_id", (user != null) ? user.SellerCategoryId ?? "" : "");
                }
                //if (principal!.GetClaim("useChat") == null)
                //{
                //    principal!.SetClaim("useChat", (user.Tenant == null) ? "false".ToLower() : user.Tenant.UseChat.ToString().ToLower());
                //}


                var roles = await _userManager.GetRolesAsync(user);
                var roledictionary = new Dictionary<string, object>();
                foreach (var role in roles)
                {
                    var permissions = new List<string>();
                    var approle = await _roleManager.FindByNameAsync(role);

                    var allClaims = await _roleManager.GetClaimsAsync(approle!);
                    foreach (var claim in allClaims)
                    {
                        permissions.Add($"{claim.Value}");
                    }
                    roledictionary.Add(role, permissions);
                }
                var rolestring = JsonSerializer.Serialize(roledictionary);


                // Override the user claims present in the principal in case they
                // changed since the authorization code/refresh token was issued.
                identity.SetClaim(Claims.Subject, await _userManager.GetUserIdAsync(user))
                        .SetClaim(Claims.Email, await _userManager.GetEmailAsync(user))
                        .SetClaim(Claims.Name, await _userManager.GetUserNameAsync(user))
                        .SetClaims(Claims.Role, (await _userManager.GetRolesAsync(user)).ToImmutableArray())
                        .SetClaim("role_permission", rolestring);

                identity.SetDestinations(GetDestinations);

                // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
                return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            if (request.GrantType == "manual" || request.IsPasswordGrantType())
            {
                return await TokensForManualGrantType(request);
            }
            throw new InvalidOperationException("The specified grant type is not supported.");
        }

        private static IEnumerable<string> GetDestinations(Claim claim)
        {
            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.
            if (claim.Subject is not null)
            {
                switch (claim.Type)
                {

                    case Claims.Name:
                    case "first_name":
                    case "last_name":
                    case "category_id":
                    case Claims.Picture:
                        yield return Destinations.AccessToken;

                        if (claim.Subject.HasScope(Scopes.Profile))
                            yield return Destinations.IdentityToken;

                        yield break;

                    case Claims.Email:
                        yield return Destinations.AccessToken;

                        if (claim.Subject.HasScope(Scopes.Email))
                            yield return Destinations.IdentityToken;

                        yield break;

                    case Claims.Role:
                        yield return Destinations.AccessToken;

                        if (claim.Subject.HasScope(Scopes.Roles))
                            yield return Destinations.IdentityToken;

                        yield break;

                    case "role_permission":
                        yield return Destinations.AccessToken;

                        if (claim.Subject.HasScope("role_permission"))
                        {
                            yield return Destinations.IdentityToken;
                        }

                        yield break;

                    // Never include the security stamp in the access and identity tokens, as it's a secret value.
                    case "AspNet.Identity.SecurityStamp": yield break;

                    default:
                        yield return Destinations.AccessToken;
                        yield break;
                }
            }
        }

        private async Task<IActionResult> TokensForManualGrantType(OpenIddictRequest request)
        {
            if (request.Username.IsNullOrEmpty() || request.Password.IsNullOrEmpty())
            {
                var properties = new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidRequestObject,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "Tidak ada Username & Pasword."
                });

                return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            AppUser? user = await _userManager.FindByEmailAsync(request.Username!);

            if (user == null || !user.Active)
            {
                var properties = new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "Account tidak terdaftar atau tidak aktif."
                });

                return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            // Validate the username/password parameters and ensure the account is not locked out.
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password!, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                var properties = new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "Username/password tidak sesuai."
                });

                return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }


            //await _signInManager.SignInAsync(user, false);
            ///////////////////////////////////////////////////

            // Retrieve the application details from the database.
            var application = await _applicationManager.FindByClientIdAsync(request.ClientId!) ??
                throw new InvalidOperationException("Details concerning the calling client application cannot be found.");

            // Retrieve the permanent authorizations associated with the user and the calling client application.
            var authorizations = await _authorizationManager.FindAsync(
                subject: await _userManager.GetUserIdAsync(user),
                client: await _applicationManager.GetIdAsync(application) ?? "",
                status: Statuses.Valid,
                type: AuthorizationTypes.Permanent,
                scopes: request.GetScopes()).ToListAsync();

            // Note: the same check is already made in the other action but is repeated
            // here to ensure a malicious user can't abuse this POST-only endpoint and
            // force it to return a valid response without the external authorization.
            if (!authorizations.Any() && await _applicationManager.HasConsentTypeAsync(application, ConsentTypes.External))
            {
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.ConsentRequired,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "The logged in user is not allowed to access this client application."
                    }));
            }

            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            if (principal.GetClaim(Claims.Picture) == null)
            {
                principal.AddClaim(Claims.Picture, (user != null) ? user.ImageUrl ?? "" : "");
            }
            if (principal.GetClaim("first_name") == null)
            {
                principal.AddClaim("first_name", (user != null) ? user.FirstName ?? "" : "");
            }
            if (principal.GetClaim("last_name") == null)
            {
                principal.AddClaim("last_name", (user != null) ? user.LastName ?? "" : "");
            }
            if (principal.GetClaim("category_id") == null)
            {
                principal.AddClaim("category_id", (user != null) ? user.SellerCategoryId ?? "" : "");
            }
            //if (principal!.GetClaim("useChat") == null)
            //{
            //    principal!.SetClaim("useChat", (user.Tenant == null) ? "false".ToLower() : user.Tenant.UseChat.ToString().ToLower());
            //}

            if (principal!.GetClaim(Claims.Role) == null)
            {
                principal!.SetClaim(Claims.Role, JsonSerializer.Serialize(await _userManager.GetRolesAsync(user)));
            }
            else
            {
                principal!.SetClaim(Claims.Role, JsonSerializer.Serialize(await _userManager.GetRolesAsync(user)));
            }


            var roles = await _userManager.GetRolesAsync(user);
            var roledictionary = new Dictionary<string, object>();
            foreach (var role in roles)
            {
                var permissions = new List<string>();
                var approle = await _roleManager.FindByNameAsync(role);

                var allClaims = await _roleManager.GetClaimsAsync(approle!);
                foreach (var claim in allClaims)
                {
                    permissions.Add($"{claim.Value}");
                }
                roledictionary.Add(role, permissions);
            }
            var rolestring = JsonSerializer.Serialize(roledictionary);
            principal.SetClaim("role_permission", rolestring);

            // Note: in this sample, the granted scopes match the requested scope
            // but you may want to allow the user to uncheck specific scopes.
            // For that, simply restrict the list of scopes before calling SetScopes.
            principal.SetScopes(request.GetScopes());
            principal.SetResources(await _scopeManager.ListResourcesAsync(principal.GetScopes()).ToListAsync());

            // Automatically create a permanent authorization to avoid requiring explicit consent
            // for future authorization or token requests containing the same scopes.
            var authorization = authorizations.LastOrDefault();
            authorization ??= await _authorizationManager.CreateAsync(
                    principal: principal,
                    subject: await _userManager.GetUserIdAsync(user),
                    client: await _applicationManager.GetIdAsync(application) ?? "",
                    type: AuthorizationTypes.Permanent,
                    scopes: principal.GetScopes());

            principal.SetAuthorizationId(await _authorizationManager.GetIdAsync(authorization));

            foreach (var claim in principal.Claims)
            {
                //claim.SetDestinations(GetDestinations(claim, principal));
                claim.SetDestinations(GetDestinations(claim));
            }

            // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        }
    }
}
