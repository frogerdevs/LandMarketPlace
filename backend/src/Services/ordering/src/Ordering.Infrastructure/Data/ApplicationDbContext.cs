using Ordering.Domain.Entities.Base;
using Ordering.Infrastructure.Data.Mappings;

namespace Ordering.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        #region Property

        #endregion
        #region DBSet
        public DbSet<BenefitCart>? BenefitCarts { get; set; } = null!;
        public DbSet<BenefitOrder>? Subscriptions { get; set; } = null!;
        public DbSet<OrderPackage>? SubscriptionDetails { get; set; } = null!;
        public DbSet<OrderSubscribe>? UserSubscriptions { get; set; } = null!;
        public DbSet<OrderUnitItem>? UserSubscriptionHistories { get; set; } = null!;

        #endregion
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Mapping
            modelBuilder.ApplyConfiguration(new OrderUnitItemMap());
            modelBuilder.ApplyConfiguration(new OrderPackageMap());
            modelBuilder.ApplyConfiguration(new OrderSubscriptionMap());
            modelBuilder.ApplyConfiguration(new BenefitOrderMap());
            modelBuilder.ApplyConfiguration(new BenefitCartMap());

            #endregion
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entitiesAdd = ChangeTracker.Entries<BaseAuditableEntity<string>>()
            .Where(e => e.State == EntityState.Added)
            .Select(e => e.Entity);

            foreach (var entity in entitiesAdd)
            {
                if (entity.CreatedDate == default)
                    entity.CreatedDate = DateTime.UtcNow;

                //if (string.IsNullOrEmpty(entity.CreatedBy))
                //    entity.CreatedBy = null;// GetCurrentUser();
            }
            var entitiesModif = ChangeTracker.Entries<BaseAuditableEntity<string>>()
            .Where(e => e.State == EntityState.Modified)
            .Select(e => e.Entity);

            foreach (var entity in entitiesModif)
            {
                if (entity.UpdatedDate == default)
                    entity.UpdatedDate = DateTime.UtcNow;

                //if (string.IsNullOrEmpty(entity.CreatedBy))
                //    entity.UpdatedBy = null;// GetCurrentUser();
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }

}
