using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace Subscription.Api.Extensions.Startup
{
    internal static class SwaggerExtension
    {
        #region ConfigureService

        internal static void AddSwagger(this IServiceCollection services, AppSettings appSettings)
        {

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(option =>
            {
                option.OperationFilter<SwaggerDefaultValues>();
                option.OperationFilter<AuthorizeCheckOperationFilter>();

                //TODO - Lowercase Swagger Documents
                //c.DocumentFilter<LowercaseDocumentFilter>();

                // include all project's xml comments
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (!assembly.IsDynamic)
                    {
                        var xmlFile = $"{assembly.GetName().Name}.xml";
                        var xmlPath = Path.Combine(baseDirectory, xmlFile);
                        if (File.Exists(xmlPath))
                        {
                            option.IncludeXmlComments(xmlPath);
                        }
                    }
                }

                //option.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                //{
                //    Type = SecuritySchemeType.OAuth2,
                //    Flows = new OpenApiOAuthFlows()
                //    {
                //        AuthorizationCode = new OpenApiOAuthFlow()
                //        {
                //            AuthorizationUrl = new Uri($"{appSettings.UrlAuthority}/connect/authorize"),
                //            TokenUrl = new Uri($"{appSettings.UrlAuthority}/connect/token"),

                //            Scopes = appSettings.OAuthSwaggerScopes.ToDictionary(x => x)
                //        }
                //    }
                //});

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
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"../swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }

                //options.OAuthClientId(appSettings.OAuthSwaggerClientId);
                //options.OAuthClientSecret(appSettings.OAuthSwaggerClientSecret);
                //options.OAuthRealm(string.Empty);
                //options.OAuthAppName("Core Api Swagger UI");
                //options.OAuthScopeSeparator(" ");
                //options.OAuthScopes(appSettings.OAuthSwaggerScopes.ToArray());
                //options.OAuthUsePkce();

                options.RoutePrefix = "swagger";
                options.DisplayRequestDuration();
            });
        }

        #endregion
    }

    #region ExtensionSwaggerOptions
    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    /// <remarks>This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "Time Management API",
                Version = description.ApiVersion.ToString(),
                Description = "Handle Time Management.",
                Contact = new OpenApiContact() { Name = "Admin", Email = "admin@ex.com" },
                License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
    /// <summary>
    /// Represents the Swagger/Swashbuckle operation filter used to document the implicit API version parameter.
    /// </summary>
    /// <remarks>This <see cref="IOperationFilter"/> is only required due to bugs in the <see cref="SwaggerGenerator"/>.
    /// Once they are fixed and published, this class can be removed.</remarks>
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

            if (operation.Parameters == null)
            {
                //return;
                operation.Parameters = new List<OpenApiParameter>();
            }
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
                    [ oAuthScheme ] = _appSettings.OAuthSwaggerScopes?.ToArray() // new[] { "email", "roles", "tenant", "apibff", "apicore" }
                }
            };
        }
    }
}

#endregion

