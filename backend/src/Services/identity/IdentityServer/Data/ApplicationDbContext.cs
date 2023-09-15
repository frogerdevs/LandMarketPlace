using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using OpenIddict.EntityFrameworkCore.Models;

namespace IdentityServer.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        #region Property
        //private readonly IHttpContextAccessor _contextAccessor;
        //private readonly HttpContext? _httpContext;
        #endregion

        #region DBSet
        //public DbSet<User>? User { get; set; } = null!;
        public DbSet<UserProfile>? SellerInput { get; set; } = null!;
        public DbSet<UserSubscription>? UserSubscription { get; set; } = null!;
        public DbSet<Province>? Provinces { get; set; } = null!;
        public DbSet<City>? Cities { get; set; } = null!;
        public DbSet<District>? Districts { get; set; } = null!;
        public DbSet<SubDistrict>? SubDistricts { get; set; } = null!;
        public DbSet<SellerAchievement>? SellerAchievements { get; set; } = null!;
        public DbSet<SellerEvent>? SellerEvents { get; set; } = null!;
        public DbSet<SellerEventImage>? SellerEventImages { get; set; } = null!;
        #endregion

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor contextAccessor)
        //    : base(options)
        //{
        //    _contextAccessor = contextAccessor;

        //    _httpContext = _contextAccessor.HttpContext;
        //    if (_httpContext != null)
        //    {
        //        if (_httpContext.Request.Path.ToString().ToLower() == "/health")
        //            return;

        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Mapping
            #region Identity

            modelBuilder.ApplyConfiguration(new AppUserMap());
            modelBuilder.ApplyConfiguration(new AppRoleMap());
            modelBuilder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.ToTable("UserClaims", "identity");
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.ToTable("UserLogins", "identity");
            });

            modelBuilder.Entity<IdentityUserToken<string>>(b =>
            {
                b.ToTable("UserTokens", "identity");
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(b =>
            {
                b.ToTable("RoleClaims", "identity");
            });

            modelBuilder.Entity<IdentityUserRole<string>>(b =>
            {
                b.ToTable("UserRoles", "identity");
            });
            #endregion
            #region OpenId

            modelBuilder.Entity<OpenIddictEntityFrameworkCoreApplication>(b =>
            {
                b.ToTable("OpenIddictApplications", "openid");
            });
            modelBuilder.Entity<OpenIddictEntityFrameworkCoreAuthorization>(b =>
            {
                b.ToTable("OpenIddictAuthorizations", "openid");
            });
            modelBuilder.Entity<OpenIddictEntityFrameworkCoreScope>(b =>
            {
                b.ToTable("OpenIddictScopes", "openid");
            });
            modelBuilder.Entity<OpenIddictEntityFrameworkCoreToken>(b =>
            {
                b.ToTable("OpenIddictTokens", "openid");
            });
            #endregion

            modelBuilder.ApplyConfiguration(new PermissionMap());
            modelBuilder.ApplyConfiguration(new RolePermissionMap());
            modelBuilder.ApplyConfiguration(new UserSubscriptionMap());
            modelBuilder.ApplyConfiguration(new SellerProfileMap());
            modelBuilder.ApplyConfiguration(new ProvinceMap());
            modelBuilder.ApplyConfiguration(new CityMap());
            modelBuilder.ApplyConfiguration(new DistrictMap());
            modelBuilder.ApplyConfiguration(new SubDistrictMap());
            modelBuilder.ApplyConfiguration(new SellerEventMap());
            modelBuilder.ApplyConfiguration(new SellerEventImageMap());
            modelBuilder.ApplyConfiguration(new SellerAchievementMap());

            #endregion
        }
    }
}