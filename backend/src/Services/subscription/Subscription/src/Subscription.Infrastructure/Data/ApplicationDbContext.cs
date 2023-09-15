namespace Subscription.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        #region Property
        #endregion
        #region DBSet
        public DbSet<BenefitType>? UnitTypes { get; set; } = null!;
        public DbSet<UnitItem>? UnitItems { get; set; } = null!;
        public DbSet<Package>? Packages { get; set; } = null!;
        public DbSet<PackageDetail>? PackageDetails { get; set; } = null!;
        public DbSet<Subscribe>? Subscribes { get; set; } = null!;
        public DbSet<SubscribeDetail>? SubscribeDetails { get; set; } = null!;
        public DbSet<Voucher>? Vouchers { get; set; } = null!;
        public DbSet<UserVoucher>? UserVouchers { get; set; } = null!;

        #endregion
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Mapping
            modelBuilder.ApplyConfiguration(new UnitTypeMap());
            modelBuilder.ApplyConfiguration(new UnitItemMap());
            modelBuilder.ApplyConfiguration(new PackageMap());
            modelBuilder.ApplyConfiguration(new PackageDetailMap());
            modelBuilder.ApplyConfiguration(new SubscribeMap());
            modelBuilder.ApplyConfiguration(new SubscribeDetailMap());
            modelBuilder.ApplyConfiguration(new VoucherMap());
            modelBuilder.ApplyConfiguration(new UserVoucherMap());


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

                if (string.IsNullOrEmpty(entity.CreatedBy))
                    entity.CreatedBy = null;// GetCurrentUser();
            }
            var entitiesModif = ChangeTracker.Entries<BaseAuditableEntity<string>>()
            .Where(e => e.State == EntityState.Modified)
            .Select(e => e.Entity);

            foreach (var entity in entitiesModif)
            {
                if (entity.UpdatedDate == default)
                    entity.UpdatedDate = DateTime.UtcNow;

                if (string.IsNullOrEmpty(entity.CreatedBy))
                    entity.UpdatedBy = null;// GetCurrentUser();
            }

            var entitiesDelete = ChangeTracker.Entries<BaseAuditableEntity<string>>()
            .Where(e => e.State == EntityState.Deleted)
            .Select(e => e.Entity);

            foreach (var entity in entitiesModif)
            {
                if (entity.DeleteDate == default)
                    entity.DeleteDate = DateTime.UtcNow;

                if (string.IsNullOrEmpty(entity.CreatedBy))
                    entity.DeleteBy = null;// GetCurrentUser();
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

    }

}
