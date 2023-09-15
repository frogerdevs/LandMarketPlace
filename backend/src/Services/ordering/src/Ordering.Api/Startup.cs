using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json.Serialization;

namespace Ordering.Api
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
            services.AddHealthChecks();
            services.AddApiVersion();
            services.AddSwagger(AppSettings);
            services.AddGrpc();
            services.AddGrpcReflection();

            services.AddControllers().AddJsonOptions(option =>
            {
                //option.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
                //option.JsonSerializerOptions.Converters.Add(new TimeOnlyConverter());
                //option.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                option.JsonSerializerOptions.WriteIndented = true;
            });
            services.AddDirectoryBrowser();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            //services.AddCustomAuthentication(AppSettings);
            //services.AddApplicationHttpClient(AppSettings);
            services.AddDependencyApplication(Configuration);
            services.AddDependencyInfrastructure(Configuration);
            #endregion
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region Ensure Create DB When Start
            app.UseDependencyApplication();
            app.UseDependencyInfrastructure();
            #endregion
            if (!env.IsProduction())
            {
                var verprovider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
                app.ConfigureSwagger(verprovider, AppSettings);
            }

            app.UseHttpsRedirection();

            //app.UseFileServer(new FileServerOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "sharefile")),
            //    RequestPath = "/sharefile",
            //    EnableDirectoryBrowsing = true,
            //    RedirectToAppendTrailingSlash = true
            //});

            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                #region Grpc
                endpoints.MapGrpcService<BenefitCartService>();
                //endpoints.MapGrpcService<GrpcSubscriptionTypeService>();
                endpoints.MapGrpcReflectionService();

                #endregion
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
