using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Data.Entites
{
    public class AppRole : IdentityRole
    {
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
