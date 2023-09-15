namespace Catalog.Infrastructure.Data.Mappings
{
    public class ProductDiscountMap : IEntityTypeConfiguration<ProductDiscount>
    {
        public void Configure(EntityTypeBuilder<ProductDiscount> builder)
        {
            builder.HasKey(e => new { e.Id });

            builder.ToTable("ProductDiscount");
            builder.HasIndex(e => e.Slug);
            builder.HasOne(d => d.Product).WithMany(p => p.ProductDiscounts)
                .HasForeignKey(d => d.ProductId);
        }
    }
}
