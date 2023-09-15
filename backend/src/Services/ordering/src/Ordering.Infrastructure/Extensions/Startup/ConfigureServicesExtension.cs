using Microsoft.Extensions.Configuration;
using Ordering.Application.Interfaces.Repositories.Base;
using Ordering.Infrastructure.Repositories.Base;

namespace Ordering.Infrastructure.Extensions.Startup
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

            services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>(name: "OrderingDB-check", tags: new string[] { "OrderingDB" });
            return services;

        }

    }
}