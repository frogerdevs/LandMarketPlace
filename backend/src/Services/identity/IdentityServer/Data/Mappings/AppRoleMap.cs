namespace IdentityServer.Data.Mappings
{
    public class AppRoleMap : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.ToTable("Roles", "identity");
        }
    }
}
