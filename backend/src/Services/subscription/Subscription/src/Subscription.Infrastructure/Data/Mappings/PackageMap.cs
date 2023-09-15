namespace Subscription.Infrastructure.Data.Mappings
{
    public class PackageMap : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.HasKey(e => new { e.Id });

            builder.ToTable("Package");
            builder.HasMany(c => c.PackageDetails).WithOne(c => c.Package).HasForeignKey(c => c.PackageId);
        }
    }
}
