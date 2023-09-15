namespace Catalog.Infrastructure.Data.Mappings
{
    public class AdsenseMap : IEntityTypeConfiguration<Adsense>
    {
        public void Configure(EntityTypeBuilder<Adsense> builder)
        {
            builder.HasKey(e => new { e.Id });

            builder.ToTable("Adsense");
            builder.HasIndex(e => e.ProductId);
            builder.HasOne(d => d.Product).WithMany(p => p.Adsenses)
                .HasForeignKey(d => d.ProductId);

            builder.Property(e => e.Id).ValueGeneratedNever();

        }
    }
}
