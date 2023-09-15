namespace IdentityServer.Data.Mappings
{
    public class PermissionMap : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permission", "identity").HasKey(c => c.Id);
            builder.HasIndex(c => c.Name);
        }
    }
}
