using OpenIddict.Validation.AspNetCore;
using static OpenIddict.Validation.OpenIddictValidationEvents;

namespace Subscription.Api.Extensions.Startup
{
    internal static class ConfigureServiceExtention
    {
        internal static void AddApiVersion(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });
        }
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, AppSettings appSettings)
        {
            #region openiddictvalidation
            services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
            services.AddOpenIddict()
                    .AddValidation(options =>
                    {
                        //options.AddEventHandler.AddHandler();
                        // Note: the validation handler uses OpenID Connect discovery
                        // to retrieve the address of the introspection endpoint.
                        options.SetIssuer(appSettings.UrlIssuer!);
                        options.AddAudiences(appSettings.ResourceClientId!);

                        // Configure the validation handler to use introspection and register the client
                        // credentials used when communicating with the remote introspection endpoint.
                        options.UseIntrospection()
                               .SetClientId(appSettings.ResourceClientId!)
                               .SetClientSecret(appSettings.ResourceClientSecret!);

                        // Register the System.Net.Http integration.
                        options.UseSystemNetHttp();

                        // Register the ASP.NET Core host.
                        options.UseAspNetCore();

                        options.AddEventHandler<ApplyIntrospectionRequestContext>(builder =>
                        {
                            builder.UseInlineHandler(context =>
                            {
                                var request = context.Transaction.GetHttpRequestMessage();
                                var _httpContextAccessor = services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>();
                                var tenantheader = _httpContextAccessor.HttpContext!.Request.Headers["tenant"];
                                if (!string.IsNullOrEmpty(tenantheader))
                                {
                                    request!.Headers.Add("tenant", new List<string>() { tenantheader! });
                                }
                                else
                                {
                                    string? tenant = null;
                                    if (_httpContextAccessor.HttpContext!.Request.Headers.TryGetValue("tenant", out var tenantid))
                                    {
                                        tenant = tenantid;
                                        if (tenant != null)
                                            request!.Headers.Add("tenant", tenant);
                                    }
                                }
                                return default;
                            });

                            builder.SetOrder(int.MinValue);
                        });



                    });
            #endregion

            #region IdentityModel.AspNetCore.OAuth2Introspection

            //services.AddAuthentication(OAuth2IntrospectionDefaults.AuthenticationScheme)
            //.AddOAuth2Introspection(options =>
            // {
            //     options.Authority = appSettings.UrlIssuer;
            //     options.DiscoveryPolicy.RequireKeySet = false;

            //     options.ClientId = appSettings.ResourceClientId;
            //     options.ClientSecret = appSettings.ResourceClientSecret;
            //     options.Events.OnSendingRequest =
            //     (e) =>
            //     {
            //         var _httpContextAccessor = e.HttpContext.RequestServices.GetRequiredService<IHttpContextAccessor>();
            //         var tenantheader = _httpContextAccessor.HttpContext!.Request.Headers["tenant"];
            //         if (!string.IsNullOrEmpty(tenantheader))
            //         {
            //             e.TokenIntrospectionRequest.Headers.Add("tenant", new List<string>() { tenantheader });
            //         }
            //         else
            //         {
            //             string? tenant = null;
            //             if (_httpContextAccessor.HttpContext!.Request.Headers.TryGetValue("tenant", out var tenantid))
            //             {
            //                 tenant = tenantid;
            //                 if (tenant != null)
            //                     e.TokenIntrospectionRequest.Headers.Add("tenant", tenant);
            //             }
            //         }

            //         return Task.CompletedTask;
            //     };

            // });
            #endregion

            services.AddAuthorization();
            return services;
        }

    }
}
