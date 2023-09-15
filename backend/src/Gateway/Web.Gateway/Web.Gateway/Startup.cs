using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using OpenIddict.Validation.AspNetCore;
namespace Web.Gateway
{
    public sealed class Startup
    {
        public IConfiguration Configuration { get; }
        public AppSettings AppSettings { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var appSettings = configuration.GetSection(nameof(AppSettings));
            AppSettings = appSettings.Get<AppSettings>()!;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            #region Inject Configuration
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            #endregion
            #region Extension
            services.AddHttpContextAccessor();
            services.AddOptions();
            services.AddMemoryCache();
            services.AddHealthChecks();
            services.AddHealthChecksUI(setup =>
            {
                // Set the maximum history entries by endpoint that will be served by the UI api middleware
                setup.MaximumHistoryEntriesPerEndpoint(20);
            })
                .AddInMemoryStorage();
            // Add services to the container.
            //services.AddAntiforgery(options =>
            //{
            //    options.Cookie.Name = "X-CSRF-TOKEN-COOKIE";
            //    options.HeaderName = "X-CSRF-TOKEN-HEADER";
            //    // Konfigurasi lainnya...
            //});
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            });

            #region OpenId
            services.AddOpenIddict()
                .AddValidation(options =>
                {
                    options.SetIssuer(AppSettings.UrlIssuer!);
                    options.AddAudiences(AppSettings.ResourceClientId!);

                    options.UseIntrospection()
                    .SetClientId(AppSettings.ResourceClientId!)
                    .SetClientSecret(AppSettings.ResourceClientSecret!);

                    options.UseSystemNetHttp();
                    options.UseAspNetCore();
                });
            #endregion

            services.AddEndpointsApiExplorer();
            services.AddApiVersion();
            services.AddSwagger(AppSettings);

            services.AddLogging();

            services.AddAuthorization();
            services.AddControllers().AddJsonOptions(option =>
            {
                //option.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
                //option.JsonSerializerOptions.Converters.Add(new TimeOnlyConverter());
                //option.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                option.JsonSerializerOptions.WriteIndented = true;
            });
            //services.AddDirectoryBrowser();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddApplicationHttpClient(AppSettings);
            services.AddDependency();
            services.AddGrpcServices(AppSettings);
            //services.AddCustomAuthentication(AppSettings);
            //services.AddApplicationHttpClient(AppSettings);
            //services.AddDependencyApplication(Configuration);
            //services.AddDependencyInfrastructure(Configuration);

            #endregion
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (!env.IsProduction())
            {
                var verprovider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
                app.ConfigureSwagger(verprovider, AppSettings);
            }

            //app.UseHttpsRedirection();


            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHealthChecksUI();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                //map healthcheck ui endpoing - default is /healthchecks-ui/
                endpoints.MapHealthChecksUI(config =>
                {
                    config.UIPath = "/hc-ui";

                });
            });
        }

    }
}
