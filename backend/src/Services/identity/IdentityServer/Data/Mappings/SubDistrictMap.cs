namespace IdentityServer.Data.Mappings
{
    public class SubDistrictMap : IEntityTypeConfiguration<SubDistrict>
    {
        public void Configure(EntityTypeBuilder<SubDistrict> builder)
        {
            builder.ToTable("SubDistrict").HasKey(c => c.Id);
            builder.HasOne(e => e.District).WithMany(e => e.SubDistricts)
                .HasForeignKey(c => c.DistrictId).IsRequired(true);
        }
    }
}
