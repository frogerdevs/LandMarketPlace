using Catalog.Domain.Entities.Base;
using Catalog.Domain.Entities.Certificate;
using Catalog.Domain.Entities.Facilities;

namespace Catalog.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        #region Property

        #endregion
        #region DBSet
        public DbSet<Category>? Categories { get; set; } = null!;
        public DbSet<SubCategory>? SubCategories { get; set; } = null!;
        public DbSet<Product>? Products { get; set; } = null!;
        public DbSet<ProductImage>? ProductImages { get; set; } = null!;
        public DbSet<HomeDeal>? HomeDeals { get; set; } = null!;
        public DbSet<Adsense>? Adsenses { get; set; } = null!;
        public DbSet<Facility>? Facilities { get; set; } = null!;
        public DbSet<CertificateType>? CertificateTypes { get; set; } = null!;
        public DbSet<ProductFacility>? ProductFacilities { get; set; } = null!;
        public DbSet<ProductDiscount>? ProductDiscounts { get; set; } = null!;
        public DbSet<ProductSpecification>? ProductSpecifications { get; set; } = null!;

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
            modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new SubCategoryMap());
            modelBuilder.ApplyConfiguration(new ProductMap());
            modelBuilder.ApplyConfiguration(new ProductImageMap());
            modelBuilder.ApplyConfiguration(new HomeDealMap());
            modelBuilder.ApplyConfiguration(new AdsenseMap());
            modelBuilder.ApplyConfiguration(new CertificateTypeMap());
            modelBuilder.ApplyConfiguration(new FacilityMap());
            modelBuilder.ApplyConfiguration(new ProductFacilityMap());
            modelBuilder.ApplyConfiguration(new ProductDiscountMap());
            modelBuilder.ApplyConfiguration(new ProductSpecificationMap());

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
            return await base.SaveChangesAsync(cancellationToken);
        }
    }

}
