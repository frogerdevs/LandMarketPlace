namespace Catalog.Infrastructure.Data.Mappings
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(e => new { e.Id });

            builder.ToTable("Product");
            builder.HasIndex(e => e.CategoryId);
            builder.HasIndex(e => e.UserId);
            builder.HasIndex(e => e.Slug);
            builder.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId);
            builder.HasOne(d => d.SubCategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.SubCategoryId);
            builder.HasOne(d => d.CertificateType).WithMany(p => p.Products)
                .HasForeignKey(d => d.CertificateId);
            builder.Property(e => e.Id).ValueGeneratedNever();

        }
    }

}
