namespace IdentityServer.Data.Mappings
{
    public class DistrictMap : IEntityTypeConfiguration<District>
    {
        public void Configure(EntityTypeBuilder<District> builder)
        {
            builder.ToTable("District").HasKey(c => c.Id);
            builder.HasOne(e => e.City).WithMany(e => e.Districts)
                .HasForeignKey(c => c.CityId).IsRequired(true);
        }
    }
}
