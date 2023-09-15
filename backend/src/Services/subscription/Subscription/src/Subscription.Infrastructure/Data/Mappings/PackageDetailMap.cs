namespace Subscription.Infrastructure.Data.Mappings
{
    public class PackageDetailMap : IEntityTypeConfiguration<PackageDetail>
    {
        public void Configure(EntityTypeBuilder<PackageDetail> builder)
        {
            builder.HasKey(e => new { e.PackageId, e.UnitItemId });

            builder.ToTable("PackageDetail");
            builder.HasOne(c => c.Package).WithMany(c => c.PackageDetails).HasForeignKey(c => c.PackageId);
            builder.HasOne(c => c.UnitItem).WithMany(c => c.PackageDetails).HasForeignKey(c => c.UnitItemId);
        }
    }
}
