using HealthChecks.UI.Client;
using IdentityServer.Configs;
using IdentityServer.Data;
using IdentityServer.Data.Seed;
using IdentityServer.Grpc;
using IdentityServer.Repositories.Implementations;
using IdentityServer.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using OpenIddict.Server.AspNetCore;
using Serilog;
using static OpenIddict.Abstractions.OpenIddictConstants;

var builder = WebApplication.CreateBuilder(args);
builder.Host
    .UseSerilog((builderContext, config) =>
    {
        config
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .WriteTo.Console(outputTemplate: "[{Timestamp:dd-MM-yyyy HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.Http("http://logstash:5000", queueLimitBytes: null);
    });

builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    {
//        options.UseNpgsql(connectionString);
//        options.UseOpenIddict();
//    });
builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString);
    options.UseOpenIddict();
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
        .SetIsOriginAllowed((host) => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

#region AspNetIdentity
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromHours(1));
builder.Services.Configure<IdentityOptions>(options =>
{
    // Configure Identity to use the same JWT claims as OpenIddict instead
    // of the legacy WS-Federation claims it uses by default (ClaimTypes),
    // which saves you from doing the mapping in your authorization controller.
    options.ClaimsIdentity.UserNameClaimType = Claims.Name;
    options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
    options.ClaimsIdentity.RoleClaimType = Claims.Role;
    options.ClaimsIdentity.EmailClaimType = Claims.Email;

    // Note: to require account confirmation before login,
    // register an email sender service (IEmailSender) and
    // set options.SignIn.RequireConfirmedAccount to true.
    //
    // For more information, visit https://aka.ms/aspaccountconf.
    options.SignIn.RequireConfirmedAccount = false;

    // Password settings.
    options.Password.RequireDigit = false; //configuration.GetValue<bool>("PasswordSettings:RequireDigit");
    options.Password.RequireLowercase = false; // configuration.GetValue<bool>("PasswordSettings:RequireLowercase");
    options.Password.RequireNonAlphanumeric = false; // configuration.GetValue<bool>("PasswordSettings:RequireNonAlphanumeric");
    options.Password.RequireUppercase = false; // configuration.GetValue<bool>("PasswordSettings:RequireUppercase");
    options.Password.RequiredLength = 6; // configuration.GetValue<int>("PasswordSettings:RequiredLength");
    options.Password.RequiredUniqueChars = 0; // configuration.GetValue<int>("PasswordSettings:RequiredUniqueChars");

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});
#endregion

#region OpenId
builder.Services.AddOpenIddict()
        // Register the OpenIddict core components.
        .AddCore(options =>
        {
            // Configure OpenIddict to use the Entity Framework Core stores and models.
            // Note: call ReplaceDefaultEntities() to replace the default entities.
            options.UseEntityFrameworkCore()
                   .UseDbContext<ApplicationDbContext>();
        })

        // Register the OpenIddict server components.
        .AddServer(options =>
        {
            // Enable the authorization, logout, token and userinfo endpoints.
            options.SetAuthorizationEndpointUris("/connect/authorize")
                                .SetLogoutEndpointUris("/connect/logout")
                                .SetTokenEndpointUris("/connect/token")
                                .SetUserinfoEndpointUris("/connect/userinfo")
                                .SetIntrospectionEndpointUris("/connect/introspect")
                                .SetVerificationEndpointUris("/connect/verify");

            // Mark the "email", "profile" and "roles" scopes as supported scopes.
            options.RegisterScopes(Scopes.Email, Scopes.Phone, Scopes.Profile, Scopes.Roles, "rolepermission");

            // Enable the client credentials flow.
            options.AllowClientCredentialsFlow()
                    .AllowAuthorizationCodeFlow()
                    .AllowRefreshTokenFlow()
                    .AllowPasswordFlow()
                    .RequireProofKeyForCodeExchange();
            // Custom auth flows are also supported
            options.AllowCustomFlow("manual");

            // Register the signing and encryption credentials.
            options.AddDevelopmentEncryptionCertificate()
                   .AddDevelopmentSigningCertificate();

            // Register the ASP.NET Core host and configure the ASP.NET Core options.
            options.UseAspNetCore()
                   .EnableAuthorizationEndpointPassthrough()
                   .EnableLogoutEndpointPassthrough()
                   .EnableTokenEndpointPassthrough()
                   .EnableUserinfoEndpointPassthrough()
                   .EnableStatusCodePagesIntegration()
                   .DisableTransportSecurityRequirement();
            options.SetAccessTokenLifetime(TimeSpan.FromMinutes(15))
            .SetRefreshTokenLifetime(TimeSpan.FromDays(1));
        })
        // Register the OpenIddict validation components.
        .AddValidation(options =>
        {
            // Import the configuration from the local OpenIddict server instance.
            options.UseLocalServer();

            // Register the ASP.NET Core host.
            options.UseAspNetCore();
        });
// Register the worker responsible of seeding the database with the sample clients.
// Note: in a real world application, this step should be part of a setup script.
builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<UserSeed>();
#endregion
//builder.Services.AddAntiforgery(options =>
//{
//    options.Cookie.Name = "X-CSRF-TOKEN-COOKIE";
//    options.HeaderName = "X-CSRF-TOKEN-HEADER";
//    // Konfigurasi lainnya...
//});
builder.Services.AddMemoryCache();
builder.Services.AddAuthentication()
    .AddGoogle("google", opt =>
    {
        var googleAuth = builder.Configuration.GetSection("AuthSettings:Google");
        opt.ClientId = googleAuth["ClientId"];
        opt.ClientSecret = googleAuth["ClientSecret"];
        opt.SignInScheme = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme;
        opt.CallbackPath = new PathString("/signin-google");
    });

#region Api Versioning
builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});
#endregion
builder.Services.AddSwaggerGen();

builder.Services.AddMediator(options =>
{
    options.ServiceLifetime = ServiceLifetime.Scoped;
});
builder.Services.AddScoped(typeof(IBaseRepositoryAsync<,>), typeof(BaseRepositoryAsync<,>));
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.Configure<DataProtectionTokenProviderOptions>(option => option.TokenLifespan = TimeSpan.FromHours(1));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddControllersWithViews();
builder.Services
    .AddGrpc(options =>
    {
        options.EnableDetailedErrors = true;
    });
builder.Services.AddGrpcReflection();

builder.Services.AddHealthChecks()
        .AddNpgSql(_ =>
            builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."),
            name: "IdentityDB-check",
            tags: new string[] { "IdentityDB" });
builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var verprovider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in verprovider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"../swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }

        options.RoutePrefix = "swagger";
        options.DisplayRequestDuration();
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var dbcontext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbcontext.Database.Migrate();
}
app.UseCookiePolicy(new CookiePolicyOptions()
{
    MinimumSameSitePolicy = SameSiteMode.None
});
//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapGrpcService<UserService>();
app.MapGrpcService<RegionAddressService>();
app.MapGrpcService<AboutService>();
app.MapGrpcService<InMerchantService>();
app.MapGrpcReflectionService();
app.MapHealthChecks("/hc", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapControllers();


app.MapRazorPages();

app.Run();
