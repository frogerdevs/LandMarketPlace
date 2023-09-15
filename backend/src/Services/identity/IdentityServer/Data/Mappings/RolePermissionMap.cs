namespace IdentityServer.Data.Mappings
{
    public class RolePermissionMap : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("RolePermission", "identity")
                .HasKey(c => new { c.RoleId, c.PermissionId });

            builder.HasOne(us => us.Role)
                .WithMany(b => b.RolePermissions)
                .HasForeignKey(ba => ba.RoleId);

            builder.HasOne(sb => sb.Permission)
                .WithMany(a => a.RolePermissions)
                .HasForeignKey(ba => ba.PermissionId);
        }
    }
}
