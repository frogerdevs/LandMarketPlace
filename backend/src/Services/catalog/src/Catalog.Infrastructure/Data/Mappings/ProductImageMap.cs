namespace Catalog.Infrastructure.Data.Mappings
{
    public class ProductImageMap : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.HasKey(e => new { e.Id });

            builder.ToTable("ProductImage");
            builder.HasIndex(e => e.ProductId);
            builder.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId);

            builder.Property(e => e.Id).ValueGeneratedNever();

        }
    }
}
