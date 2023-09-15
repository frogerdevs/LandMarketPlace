namespace IdentityServer.Data.Entites
{
    public class RolePermission
    {
        public string RoleId { get; set; }
        public string PermissionId { get; set; }
        public string Name { get; set; }
        public Permission Permission { get; set; }
        public AppRole Role { get; set; }
    }
}
