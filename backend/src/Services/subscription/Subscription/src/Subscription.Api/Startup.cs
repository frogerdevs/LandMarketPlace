using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Subscription.Api.Extensions.Startup;
using Subscription.Api.Grpc;
using System.Text.Json.Serialization;

namespace Subscription.Api
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

            services.AddGrpc(options =>
                    {
                        options.EnableDetailedErrors = true;
                    });
            services.AddGrpcReflection();

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
                endpoints.MapGrpcService<UnitTypeService>();
                endpoints.MapGrpcService<UnitItemService>();
                endpoints.MapGrpcService<PackageService>();
                endpoints.MapGrpcService<SubscribeService>();
                endpoints.MapGrpcService<VoucherService>();
                endpoints.MapGrpcReflectionService();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
