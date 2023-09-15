namespace Catalog.Infrastructure.Data.Mappings
{
    public class ProductFacilityMap : IEntityTypeConfiguration<ProductFacility>
    {
        public void Configure(EntityTypeBuilder<ProductFacility> builder)
        {
            builder.HasKey(e => new { e.ProductId, e.FacilityId });

            builder.ToTable("ProductFacility");
            builder.HasOne(d => d.Product).WithMany(p => p.ProductFacilities)
                .HasForeignKey(d => d.ProductId);
            builder.HasOne(d => d.Facility).WithMany(p => p.ProductFacilities)
                .HasForeignKey(d => d.FacilityId);
        }
    }
}
