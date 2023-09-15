namespace IdentityServer.Data.Entites
{
    public class Permission
    {
        public string Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
        public Permission()
        {
            Id = Guid.NewGuid().ToString();
        }
        public Permission(string id)
        {
            Id = id;
        }
    }
}
