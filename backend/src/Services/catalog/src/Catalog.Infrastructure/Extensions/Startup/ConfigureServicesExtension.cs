using Catalog.Application.Interfaces.Repositories.Base;
using Catalog.Infrastructure.Repositories.Base;
using Microsoft.Extensions.Configuration;

namespace Catalog.Infrastructure.Extensions.Startup;
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

        //services.AddHealthChecks();
        services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>(name: "CatalogDB-check", tags: new string[] { "CatalogDB" });

        return services;

    }

}

