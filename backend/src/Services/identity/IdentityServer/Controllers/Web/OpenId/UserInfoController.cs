using IdentityServer.Controllers.Web.Base;
using IdentityServer.Data.Entites;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using OpenIddict.Validation.AspNetCore;
using System.Data;
using System.Text.Json;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace IdentityServer.Controllers.Web.OpenId
{
    public class UserInfoController : BaseWebController<UserInfoController>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public UserInfoController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //
        // GET: /api/userinfo
        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        //[Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
        [HttpGet("~/connect/userinfo"), HttpPost("~/connect/userinfo"), Produces("application/json")]
        public async Task<IActionResult> UserInfo()
        {
            var user = await _userManager.FindByIdAsync(User.GetClaim(Claims.Subject) ?? "");
            if (user == null)
            {
                return Challenge(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidToken,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "The specified access token is bound to an account that no longer exists."
                    }));
            }

            var claims = new Dictionary<string, object>(StringComparer.Ordinal)
            {
                // Note: the "sub" claim is a mandatory claim and must be included in the JSON response.
                [Claims.Subject] = await _userManager.GetUserIdAsync(user)
            };

            if (User.HasScope(Scopes.Email))
            {
                claims[Claims.Email] = await _userManager.GetEmailAsync(user) ?? "";
                claims[Claims.EmailVerified] = await _userManager.IsEmailConfirmedAsync(user);
            }

            if (User.HasScope(Scopes.Phone))
            {
                claims[Claims.PhoneNumber] = await _userManager.GetPhoneNumberAsync(user) ?? "";
                claims[Claims.PhoneNumberVerified] = await _userManager.IsPhoneNumberConfirmedAsync(user);
            }
            if (User.HasScope(Scopes.Profile))
            {
                var ds = await _userManager.Users.Select(c => new { c.FirstName, c.LastName, c.Email, c.ImageUrl }).FirstOrDefaultAsync(c => c.Email == user.Email);
                claims[Claims.Name] = (ds != null) ? ds.FirstName ?? "" : "";
                claims[Claims.Picture] = (ds != null) ? ds.ImageUrl ?? "" : "";
            }
            if (User.HasScope(Scopes.Roles))
            {
                claims[Claims.Role] = await _userManager.GetRolesAsync(user);
            }
            if (User.HasScope("rolepermission"))
            {
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
                claims["rolepermission"] = await _userManager.GetRolesAsync(user);
            }

            // Note: the complete list of standard claims supported by the OpenID Connect specification
            // can be found here: http://openid.net/specs/openid-connect-core-1_0.html#StandardClaims

            return Ok(claims);
        }

    }
}
