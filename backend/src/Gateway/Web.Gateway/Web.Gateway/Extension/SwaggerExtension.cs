using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using System.Text.Json;
using Web.Gateway.Config;

namespace Web.Gateway.Extension
{
    internal static class SwaggerExtension
    {
        #region ConfigureService
        internal static void AddSwagger(this IServiceCollection services, AppSettings appSettings)
        {
            //services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<SwaggerDefaultValues>();
                c.OperationFilter<AuthorizeCheckOperationFilter>();

                #region manual set version
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (!assembly.IsDynamic)
                    {
                        var xmlFile = $"{assembly.GetName().Name}.xml";
                        var xmlPath = Path.Combine(baseDirectory, xmlFile);
                        if (File.Exists(xmlPath))
                        {
                            c.IncludeXmlComments(xmlPath);
                        }
                    }
                }
                #endregion

                #region read dynamic folder
                //// Membaca versi API dari struktur folder secara dinamis
                //var controllerAssembly = Assembly.GetExecutingAssembly();
                //var controllerTypes = controllerAssembly.GetTypes()
                //    .Where(t => t.Name.EndsWith("Controller"));

                //var versions = new HashSet<string>();
                //var serviceProvider = services.BuildServiceProvider();
                //var apiVersionDescriptionProvider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();

                //foreach (var controllerType in controllerTypes)
                //{
                //    var version = controllerType.Namespace?.Split('.').LastOrDefault()?.ToLowerInvariant();
                //    var routePrefix = $"api/{version}";

                //    if (!versions.Contains(version!))
                //    {
                //        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                //        {
                //            var info = new OpenApiInfo()
                //            {
                //                Title = $"Gateway API {version}",
                //                Version = version,
                //                Description = "Handle Gateway.",
                //                License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
                //            };

                //            if (description.IsDeprecated)
                //            {
                //                info.Description += " This API version has been deprecated.";
                //            }

                //            c.SwaggerDoc(description.GroupName, info);
                //        }

                //        versions.Add(version!);
                //    }

                //    c.DocInclusionPredicate((docName, apiDesc) =>
                //    {
                //        if (!apiDesc.TryGetMethodInfo(out var methodInfo))
                //            return false;

                //        var controllerNamespace = methodInfo.DeclaringType?.Namespace?.ToLowerInvariant();
                //        return controllerNamespace?.Contains(version!) == true;
                //    });

                //    c.TagActionsBy(apiDesc =>
                //    {
                //        var controllerNamespace = apiDesc.ActionDescriptor.RouteValues["controller"]?.ToLowerInvariant();
                //        return new[] { $"{version}/{controllerNamespace}" };
                //    });

                //    c.DocumentFilter<VersionPrefixDocumentFilter>(routePrefix);
                //    c.OperationFilter<RemoveVersionFromControllerRouteOperationFilter>();

                //}
                #endregion


                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        AuthorizationCode = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri($"{appSettings.UrlAuthority}/connect/authorize"),
                            TokenUrl = new Uri($"{appSettings.UrlAuthority}/connect/token"),

                            Scopes = appSettings.OAuthSwaggerScopes!.ToDictionary(x => x)
                        }
                    }
                });
            });
        }
        #endregion

        #region Configure
        internal static void ConfigureSwagger(this IApplicationBuilder app,
            IApiVersionDescriptionProvider provider, AppSettings appSettings)
        {
            //IServiceProvider services = app.ApplicationServices;
            //var provider = services.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                var verprovider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
                var versions = verprovider.ApiVersionDescriptions
                                .Select(description => description.GroupName.ToString())
                                .ToList();

                foreach (var version in versions)
                {
                    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"API {version}");

                    //// Menghilangkan tautan menu untuk versi yang tidak ada controller di dalamnya
                    if (!HasControllersInVersion(version))
                    {
                        c.ConfigObject.DocExpansion = DocExpansion.None; // Menyembunyikan daftar aksi
                        c.ConfigObject.DisplayRequestDuration = false; // Menyembunyikan durasi permintaan
                        c.ConfigObject.DefaultModelsExpandDepth = -1; // Menyembunyikan model
                        c.ConfigObject.ShowExtensions = false; // Menyembunyikan ekstensi Swagger
                    }

                }

                // Konfigurasi tambahan untuk UI Swagger
                c.DefaultModelsExpandDepth(-1); // Mengatur tingkat ekspansi model menjadi -1 (tidak diperluas)
                c.DocExpansion(DocExpansion.List); // Mengatur tampilan dokumen Swagger menjadi daftar
                c.RoutePrefix = "swagger";
                c.DisplayRequestDuration();


                c.OAuthClientId(appSettings.OAuthSwaggerClientId);
                c.OAuthClientSecret(appSettings.OAuthSwaggerClientSecret);
                c.OAuthRealm(string.Empty);
                c.OAuthAppName("Gateway Api Swagger UI");
                c.OAuthScopeSeparator(" ");
                c.OAuthScopes(appSettings.OAuthSwaggerScopes!.ToArray());
                c.OAuthUsePkce();
            });

        }
        static bool HasControllersInVersion(string version)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var controllerTypes = assembly.GetTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
                .Where(type => type.GetCustomAttributes<ApiControllerAttribute>().Any())
                .Where(type => type.Namespace?.ToLowerInvariant().EndsWith(version) == true);

            return controllerTypes.Any();
        }

        #endregion

    }

    #region Swagger Configuration
    public class VersionPrefixDocumentFilter : IDocumentFilter
    {
        private readonly string _routePrefix;

        public VersionPrefixDocumentFilter(string routePrefix)
        {
            _routePrefix = routePrefix;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = new OpenApiPaths();
            foreach (var path in swaggerDoc.Paths)
            {
                var newPath = path.Key.Replace("{{version}}", _routePrefix);
                paths.Add(newPath, path.Value);
            }

            swaggerDoc.Paths = paths;
        }
    }

    public class RemoveVersionFromControllerRouteOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var controllerActionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

            if (controllerActionDescriptor != null)
            {
                var controllerNamespace = controllerActionDescriptor.ControllerTypeInfo.Namespace;
                var version = controllerNamespace?.Split('.').LastOrDefault()?.ToLowerInvariant();

                if (!string.IsNullOrEmpty(version))
                {
                    var routePrefix = $"api/{version}";
                    if (!operation.Tags.First().Name.Contains(routePrefix))
                    {

                        var updatedPath = routePrefix + "/" + operation.Tags.First().Name.Replace("[controller]", string.Empty).Replace(version, string.Empty).TrimStart('/');
                        operation.Tags.First().Name = updatedPath;
                        operation.Tags.First().Description = $"API {version}";
                    }
                }
            }
        }
    }
    public class SwaggerDefaultValues : IOperationFilter
    {
        /// <summary>
        /// Applies the filter to the specified operation using the given context.
        /// </summary>
        /// <param name="operation">The operation to apply the filter to.</param>
        /// <param name="context">The current operation filter context.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;

            operation.Deprecated |= apiDescription.IsDeprecated();

            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1752#issue-663991077
            foreach (var responseType in context.ApiDescription.SupportedResponseTypes)
            {
                // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/b7cf75e7905050305b115dd96640ddd6e74c7ac9/src/Swashbuckle.AspNetCore.SwaggerGen/SwaggerGenerator/SwaggerGenerator.cs#L383-L387
                var responseKey = responseType.IsDefaultResponse ? "default" : responseType.StatusCode.ToString();
                var response = operation.Responses[responseKey];

                foreach (var contentType in response.Content.Keys)
                {
                    if (!responseType.ApiResponseFormats.Any(x => x.MediaType == contentType))
                    {
                        response.Content.Remove(contentType);
                    }
                }
            }

            //if (operation.Parameters == null)
            //{
            //    //return;
            //    operation.Parameters = new List<OpenApiParameter>();
            //}
            //operation.Parameters.Add(new OpenApiParameter
            //{
            //    Name = "tenant",
            //    In = ParameterLocation.Header,
            //    Required = true, // set to false if this is optional
            //    Schema = new OpenApiSchema
            //    {
            //        Type = "string"
            //    }
            //});

            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/412
            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/pull/413
            foreach (var parameter in operation.Parameters)
            {
                if (parameter.In == ParameterLocation.Header ||
                        apiDescription.ParameterDescriptions == null ||
                        apiDescription.ParameterDescriptions.Count == 0)
                    continue;

                var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

                if (parameter.Description == null)
                {
                    parameter.Description = description.ModelMetadata?.Description;
                }

                if (parameter.Schema.Default == null && description.DefaultValue != null)
                {
                    // REF: https://github.com/Microsoft/aspnet-api-versioning/issues/429#issuecomment-605402330
                    var json = JsonSerializer.Serialize(description.DefaultValue, description.ModelMetadata!.ModelType);
                    parameter.Schema.Default = OpenApiAnyFactory.CreateFromJson(json);
                }

                parameter.Required |= description.IsRequired;
            }
        }
    }
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        private readonly AppSettings _appSettings;
        public AuthorizeCheckOperationFilter(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check for authorize attribute
            var hasAuthorize = context.MethodInfo.DeclaringType!.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                                context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            if (!hasAuthorize) return;

            //operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
            //operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

            var oAuthScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            };

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    [ oAuthScheme ] = _appSettings.OAuthSwaggerScopes!.ToArray() // new[] { "email", "roles", "tenant", "apibff", "apicore" }
                }
            };
        }
    }

    #endregion
}
