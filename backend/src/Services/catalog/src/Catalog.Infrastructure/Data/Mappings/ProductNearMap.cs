namespace Catalog.Infrastructure.Data.Mappings
{
    public class ProductNearMap : IEntityTypeConfiguration<ProductNear>
    {
        public void Configure(EntityTypeBuilder<ProductNear> builder)
        {
            builder.HasKey(e => new { e.Id });

            builder.ToTable("ProductNear");
            builder.HasOne(d => d.Product).WithMany(p => p.ProductNears)
                .HasForeignKey(d => d.ProductId);
        }
    }
}
