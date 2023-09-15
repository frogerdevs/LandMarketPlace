using Catalog.Api.Extensions.Startup;
using Catalog.Api.Grpc;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json.Serialization;

namespace Catalog.Api
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
            //services.AddHealthChecks();
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
            services
                .AddGrpc(options =>
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

            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<CategoryService>();
                endpoints.MapGrpcService<HomeService>();
                endpoints.MapGrpcService<ProductService>();
                endpoints.MapGrpcService<ProductDiscountService>();
                endpoints.MapGrpcService<AdsenseService>();
                endpoints.MapGrpcService<HomeDealService>();
                endpoints.MapGrpcService<FacilityService>();
                endpoints.MapGrpcReflectionService();

                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                //endpoints.MapHealthChecks("/hc", new HealthCheckOptions
                //{
                //    Predicate = check => check.Tags.Contains("CatalogDB"),
                //    //Predicate = _ => true,
                //    ResponseWriter = async (context, report) =>
                //    {
                //        if (report != null)
                //        {
                //            context.Response.ContentType = "application/json";
                //            UIHealthReport value = UIHealthReport.CreateFrom(report);
                //            await JsonSerializer.SerializeAsync(context.Response.Body, value, _options.Value);
                //        }
                //        else
                //        {
                //            await context.Response.BodyWriter.WriteAsync("{}"u8.ToArray());
                //        }
                //    }
                //}); 
            });
        }


    }
}
