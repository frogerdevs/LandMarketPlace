namespace Catalog.Infrastructure.Data.Mappings
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(e => new { e.Id });

            builder.ToTable("Category");

            builder.HasIndex(e => e.Slug);

            builder.Property(e => e.Id).IsRequired();
            builder.Property(e => e.Name)
                .HasColumnName("Name");
            builder.Property(e => e.Description);
            builder.Property(e => e.ImageUrl);
            builder.Property(e => e.Active);


        }

    }
}
