namespace Catalog.Infrastructure.Extensions.Startup;
public static class ConfigureExtension
{
    public static void UseDependencyInfrastructure(this IApplicationBuilder app)
    {
        #region Ensure Create DB When Start
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        //dbContext.Database.EnsureCreated();
        dbContext.Database.Migrate();
        #endregion

    }
}

