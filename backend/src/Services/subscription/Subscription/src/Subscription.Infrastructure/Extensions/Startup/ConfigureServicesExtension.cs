using Microsoft.Extensions.Configuration;
using Subscription.Application.Interfaces.Repositories.Base;

namespace Subscription.Infrastructure.Extensions.Startup
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection AddDependencyInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            //services.AddDbContext<ApplicationDbContext>();
            services.AddDbContextPool<ApplicationDbContext>(
                o => o.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IBaseRepositoryAsync<,>), typeof(BaseRepositoryAsync<,>));
            //services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
            //services.AddScoped<ICreateDBTenant, CreateDBTenant>();
            //services.AddScoped<ISendEmail, SendEmail>();

            services.AddHostedService<BenefitTypeData>();


            services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>(name: "DB-check", tags: new string[] { "DB" });
            return services;

        }

    }
}