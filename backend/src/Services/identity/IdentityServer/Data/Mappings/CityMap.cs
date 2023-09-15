namespace IdentityServer.Data.Mappings
{
    public class CityMap : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("City").HasKey(c => c.Id);
            builder.HasOne(e => e.Province).WithMany(e => e.Cities)
                .HasForeignKey(c => c.ProvinceId).IsRequired(true);
        }
    }

}
