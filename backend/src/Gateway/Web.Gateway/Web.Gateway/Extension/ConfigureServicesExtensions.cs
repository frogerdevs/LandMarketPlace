using GrpcCatalog;
using GrpcIdentity;
using GrpcOrdering;
using GrpcSubscription;
using Microsoft.AspNetCore.Mvc.Versioning;
using Polly;
using System.Reflection;

namespace Web.Gateway.Extension
{
    internal static class ConfigureServicesExtensions
    {
        internal static void AddApiVersion(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        }
        public static IServiceCollection AddApplicationHttpClient(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddTransient<HttpClientChannelDelegatingHandler>();
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();
            #region HttpClientIdentity
            services.AddHttpClient("Identity", httpClient =>
            {
                var baseaddress = appSettings!.Urls!.Identity!.EndsWith('/') ? appSettings.Urls.Identity : appSettings.Urls.Identity + "/";
                httpClient.BaseAddress = new Uri(baseaddress);
                // Add a user-agent default request header.
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("gateway");
            })
                .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                })
                .AddHttpMessageHandler<HttpClientChannelDelegatingHandler>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddTransientHttpErrorPolicy(policyBuilder =>
                        policyBuilder.RetryAsync(1))
                .AddTransientHttpErrorPolicy(policyBuilder =>
                        policyBuilder.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
            #endregion

            #region HttpClientCatalog
            services.AddHttpClient("Catalog", httpClient =>
            {
                var baseaddress = appSettings!.Urls!.Catalog!.EndsWith('/') ? appSettings.Urls.Catalog : appSettings.Urls.Catalog + "/";
                httpClient.BaseAddress = new Uri(baseaddress);
            })
                .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                })
                .AddHttpMessageHandler<HttpClientChannelDelegatingHandler>()
                .AddTransientHttpErrorPolicy(policyBuilder =>
                        policyBuilder.RetryAsync(1))
                .AddTransientHttpErrorPolicy(policyBuilder =>
                        policyBuilder.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
            #endregion

            #region HttpClientInspiration
            services.AddHttpClient("Inspiration", httpClient =>
            {
                var baseaddress = appSettings!.Urls!.Inspiration!.EndsWith('/') ? appSettings.Urls.Inspiration : appSettings.Urls.Inspiration + "/";
                httpClient.BaseAddress = new Uri(baseaddress);
            })
                .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                })
                .AddHttpMessageHandler<HttpClientChannelDelegatingHandler>()
                .AddTransientHttpErrorPolicy(policyBuilder =>
                        policyBuilder.RetryAsync(1))
                .AddTransientHttpErrorPolicy(policyBuilder =>
                        policyBuilder.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
            #endregion
            #region HttpClientTestimonial
            services.AddHttpClient("Testimonial", httpClient =>
            {
                var baseaddress = appSettings!.Urls!.Testimonial!.EndsWith('/') ? appSettings.Urls.Testimonial : appSettings.Urls.Testimonial + "/";
                httpClient.BaseAddress = new Uri(baseaddress);
            })
                .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                })
                .AddHttpMessageHandler<HttpClientChannelDelegatingHandler>()
                .AddTransientHttpErrorPolicy(policyBuilder =>
                        policyBuilder.RetryAsync(1))
                .AddTransientHttpErrorPolicy(policyBuilder =>
                        policyBuilder.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
            #endregion
            #region HttpClientExhibition
            services.AddHttpClient("Exhibition", httpClient =>
            {
                var baseaddress = appSettings!.Urls!.Exhibition!.EndsWith('/') ? appSettings.Urls.Exhibition : appSettings.Urls.Exhibition + "/";
                httpClient.BaseAddress = new Uri(baseaddress);
            })
                .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                })
                .AddHttpMessageHandler<HttpClientChannelDelegatingHandler>()
                .AddTransientHttpErrorPolicy(policyBuilder =>
                        policyBuilder.RetryAsync(1))
                .AddTransientHttpErrorPolicy(policyBuilder =>
                        policyBuilder.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
            #endregion
            #region HttpClientAnalytic
            services.AddHttpClient("Analytic", httpClient =>
            {
                var baseaddress = appSettings!.Urls!.Analytic!.EndsWith('/') ? appSettings.Urls.Analytic : appSettings.Urls.Analytic + "/";
                httpClient.BaseAddress = new Uri(baseaddress);
            })
                .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                })
                .AddHttpMessageHandler<HttpClientChannelDelegatingHandler>()
                .AddTransientHttpErrorPolicy(policyBuilder =>
                        policyBuilder.RetryAsync(1))
                .AddTransientHttpErrorPolicy(policyBuilder =>
                        policyBuilder.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
            #endregion

            return services;
        }

        public static IServiceCollection AddDependency(this IServiceCollection services)
        {

            //services.AddScoped<IProductsService, ProductService>();
            //services.AddScoped<ISendEmail, SendEmail>();

            services.Scan(scan => scan
                    .FromAssemblyDependencies(Assembly.GetExecutingAssembly())
                    .AddClasses(classes =>
                        classes.AssignableTo<IScopedDependency>())
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

            services.Scan(scan => scan
                    .FromAssemblyDependencies(Assembly.GetExecutingAssembly())
                    .AddClasses(classes =>
                        classes.AssignableTo<ITransientDependency>())
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

            services.Scan(scan => scan
                    .FromAssemblyDependencies(Assembly.GetExecutingAssembly())
                    .AddClasses(classes =>
                        classes.AssignableTo<ISingletonDependency>())
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime());

            return services;

        }
        public static IServiceCollection AddGrpcServices(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddTransient<GrpcExceptionInterceptor>();

            #region Identity
            services.AddGrpcClient<UserGrpc.UserGrpcClient>((services, options) =>
            {
                var url = appSettings!.Urls!.GrpcIdentity;
                options.Address = new Uri(url!);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            services.AddGrpcClient<RegionAddressGrpc.RegionAddressGrpcClient>((services, options) =>
            {
                var url = appSettings!.Urls!.GrpcIdentity;
                options.Address = new Uri(url!);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            services.AddGrpcClient<AboutGrpc.AboutGrpcClient>((services, options) =>
            {
                var url = appSettings!.Urls!.GrpcIdentity;
                options.Address = new Uri(url!);
            }).AddInterceptor<GrpcExceptionInterceptor>();
            services.AddGrpcClient<InMerchantGrpc.InMerchantGrpcClient>((services, options) =>
            {
                var url = appSettings!.Urls!.GrpcIdentity;
                options.Address = new Uri(url!);
            }).AddInterceptor<GrpcExceptionInterceptor>();
            #endregion

            #region Catalog

            services.AddGrpcClient<CategoryGrpc.CategoryGrpcClient>((services, options) =>
            {
                var url = appSettings!.Urls!.GrpcCatalog;
                options.Address = new Uri(url!);
            }).AddInterceptor<GrpcExceptionInterceptor>();
            services.AddGrpcClient<ProductGrpc.ProductGrpcClient>((services, options) =>
            {
                var url = appSettings!.Urls!.GrpcCatalog;
                options.Address = new Uri(url!);
            }).AddInterceptor<GrpcExceptionInterceptor>();
            services.AddGrpcClient<HomeGrpc.HomeGrpcClient>((services, options) =>
            {
                var url = appSettings!.Urls!.GrpcCatalog;
                options.Address = new Uri(url!);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            services.AddGrpcClient<ProductDiscountGrpc.ProductDiscountGrpcClient>((services, options) =>
            {
                var url = appSettings!.Urls!.GrpcCatalog;
                options.Address = new Uri(url!);
            }).AddInterceptor<GrpcExceptionInterceptor>();
            #endregion

            #region Subscription

            services.AddGrpcClient<UnitTypeGrpc.UnitTypeGrpcClient>((services, options) =>
            {
                var url = appSettings!.Urls!.GrpcSubscription;
                options.Address = new Uri(url!);
            }).AddInterceptor<GrpcExceptionInterceptor>();
            #endregion

            #region Ordering

            services.AddGrpcClient<BenefitCartGrpc.BenefitCartGrpcClient>((services, options) =>
            {
                var url = appSettings!.Urls!.GrpcOrdering;
                options.Address = new Uri(url!);
            }).AddInterceptor<GrpcExceptionInterceptor>();
            #endregion


            return services;
        }
    }
}
