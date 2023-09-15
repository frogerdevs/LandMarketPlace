namespace Catalog.Infrastructure.Data.Mappings
{
    public class ProductSpecificationMap : IEntityTypeConfiguration<ProductSpecification>
    {
        public void Configure(EntityTypeBuilder<ProductSpecification> builder)
        {
            builder.HasKey(e => new { e.Id });

            builder.ToTable("ProductSpecification");
            builder.HasOne(d => d.Product).WithMany(p => p.ProductSpecifications)
                .HasForeignKey(d => d.ProductId);
        }
    }
}
