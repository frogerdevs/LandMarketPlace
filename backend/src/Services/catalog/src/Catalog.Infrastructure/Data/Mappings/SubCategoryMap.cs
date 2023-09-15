namespace Catalog.Infrastructure.Data.Mappings
{
    public class SubCategoryMap : IEntityTypeConfiguration<SubCategory>
    {
        public void Configure(EntityTypeBuilder<SubCategory> builder)
        {
            builder.HasKey(e => new { e.Id });

            builder.ToTable("SubCategory");

            builder.Property(e => e.Id).IsRequired();

        }

    }
}
