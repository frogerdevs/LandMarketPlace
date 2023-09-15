namespace IdentityServer.Data.Mappings
{
    public class ProvinceMap : IEntityTypeConfiguration<Province>
    {
        public void Configure(EntityTypeBuilder<Province> builder)
        {
            builder.ToTable("Province").HasKey(c => c.Id);

        }
    }
}
