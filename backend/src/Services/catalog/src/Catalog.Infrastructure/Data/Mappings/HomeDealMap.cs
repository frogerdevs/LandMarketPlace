namespace Catalog.Infrastructure.Data.Mappings
{
    public class HomeDealMap : IEntityTypeConfiguration<HomeDeal>
    {
        public void Configure(EntityTypeBuilder<HomeDeal> builder)
        {
            builder.HasKey(e => new { e.Id });

            builder.ToTable("HomeDeal");
            builder.HasOne(d => d.Product).WithMany(p => p.HomeDeals)
                .HasForeignKey("ProductId");
        }

    }
}
