namespace Catalog.Infrastructure.Data.Mappings
{
    public class ProductNearItemMap : IEntityTypeConfiguration<ProductNearItem>
    {
        public void Configure(EntityTypeBuilder<ProductNearItem> builder)
        {
            builder.HasKey(e => new { e.Id });

            builder.ToTable("ProductNearItem");
            builder.HasOne(d => d.ProductNear).WithMany(p => p.ProductNearItems)
                .HasForeignKey(d => d.ProductNearId);

        }

    }
}
