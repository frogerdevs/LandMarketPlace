using OpenIddict.Abstractions;
using System.Globalization;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace IdentityServer.Data.Seed
{
    public class Worker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        public Worker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            #region Postman
            string postmanClient = "PostManClient";

            var pstman = await manager.FindByClientIdAsync(postmanClient);
            if (pstman != null)
            {
                await manager.DeleteAsync(pstman);
            }
            if (await manager.FindByClientIdAsync(postmanClient) == null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = postmanClient,
                    ClientSecret = "PostMan-Secreet",
                    ConsentType = ConsentTypes.Explicit,
                    DisplayName = "Postman UI Application",
                    RedirectUris = { new Uri("https://oauth.pstmn.io/v1/callback") },
                    //PostLogoutRedirectUris = { new Uri("https://oauth.pstmn.io/v1/callback") },
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Logout,
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.GrantTypes.ClientCredentials,
                        Permissions.GrantTypes.Password,
                        Permissions.Prefixes.GrantType + "manual",
                        Permissions.ResponseTypes.Code,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,
                        Permissions.Prefixes.Scope + "apibff",
                        Permissions.Prefixes.Scope + "rolepermission",
                    },
                    Requirements = { Requirements.Features.ProofKeyForCodeExchange }

                });
            }
            #endregion

            #region BFF
            string bffClient = "BffClientSwagger";

            var bff = await manager.FindByClientIdAsync(bffClient);
            if (bff != null)
            {
                await manager.DeleteAsync(bff);
            }
            if (await manager.FindByClientIdAsync(bffClient) == null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = bffClient,
                    ClientSecret = "BffClient-Secreet",
                    ConsentType = ConsentTypes.Explicit,
                    DisplayName = "BFF UI Application",
                    RedirectUris = { new Uri("http://localhost:7405/swagger/oauth2-redirect.html") },
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Logout,
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.GrantTypes.ClientCredentials,
                        Permissions.GrantTypes.Password,
                        Permissions.Prefixes.GrantType + "manual",
                        Permissions.ResponseTypes.Code,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,
                        Permissions.Prefixes.Scope + "apibff",
                        Permissions.Prefixes.Scope + "rolepermission",
                    },
                    Requirements = { Requirements.Features.ProofKeyForCodeExchange }

                });
            }

            if (await manager.FindByClientIdAsync("Resource_Bff") == null)
            {
                var desriptor = new OpenIddictApplicationDescriptor
                {
                    ClientId = "Resource_Bff",
                    ClientSecret = "Resource-Bff-Secret",
                    Permissions = { Permissions.Endpoints.Introspection }
                };
                await manager.CreateAsync(desriptor);
            }

            var scopemanager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();
            if (await scopemanager.FindByNameAsync("apibff") is null)
            {
                await scopemanager.CreateAsync(new OpenIddictScopeDescriptor
                {
                    DisplayName = "BFF API Access",
                    DisplayNames = { [CultureInfo.GetCultureInfo("id-ID")] = " BFF API" },
                    Name = "apibff",
                    Resources = { "Resource_Bff" }
                });
            }
            #endregion
            #region Manual
            string bffManual = "ManualCredential";

            var manualCred = await manager.FindByClientIdAsync(bffManual);
            if (manualCred != null)
            {
                await manager.DeleteAsync(manualCred, cancellationToken);
            }
            if (await manager.FindByClientIdAsync(bffManual, cancellationToken) == null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = bffManual,
                    ClientSecret = "ManualCredential-Secreet",
                    ConsentType = ConsentTypes.Explicit,
                    DisplayName = "ManualCredential Application",
                    RedirectUris = { new Uri("http://192.168.100.78:7405/swagger/oauth2-redirect.html") },
                    PostLogoutRedirectUris = { },
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Logout,
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.GrantTypes.ClientCredentials,
                        Permissions.GrantTypes.Password,
                        Permissions.Prefixes.GrantType + "manual",
                        Permissions.ResponseTypes.Code,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,
                        Permissions.Prefixes.Scope + "apibff",
                        Permissions.Prefixes.Scope + "rolepermission",
                    },
                    Requirements = { Requirements.Features.ProofKeyForCodeExchange },

                }, cancellationToken);
            }
            #endregion
            #region NextJsCredential
            string nextjsCredential = "NextJsCredential";

            var nextjsCred = await manager.FindByClientIdAsync(nextjsCredential, cancellationToken);
            if (nextjsCred != null)
            {
                await manager.DeleteAsync(nextjsCred, cancellationToken);
            }
            if (await manager.FindByClientIdAsync(nextjsCredential, cancellationToken) == null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = nextjsCredential,
                    ClientSecret = "NextJsCredential-Secreet",
                    ConsentType = ConsentTypes.Explicit,
                    DisplayName = "NextJs Credential Application",
                    RedirectUris = { new Uri("http://localhost:3000/user") },
                    PostLogoutRedirectUris = { new Uri("http://localhost:3000") },
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Logout,
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.GrantTypes.ClientCredentials,
                        Permissions.GrantTypes.Password,
                        Permissions.Prefixes.GrantType + "manual",
                        Permissions.ResponseTypes.Code,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,
                        Permissions.Prefixes.Scope + "apibff",
                        Permissions.Prefixes.Scope + "rolepermission",
                    },
                    Requirements = { Requirements.Features.ProofKeyForCodeExchange },

                }, cancellationToken);
            }
            #endregion

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
