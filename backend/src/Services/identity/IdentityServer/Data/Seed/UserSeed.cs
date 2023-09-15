using IdentityServer.Data.Entites;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityServer.Data.Seed
{
    public class UserSeed : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<UserSeed> _logger;
        public UserSeed(IServiceProvider serviceProvider, ILogger<UserSeed> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await context.Database.EnsureCreatedAsync();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

                await SeedRoleSystemAdmin(roleManager);
                await SeedRoleBasicUser(roleManager);
                await SeedRoleMerchant(roleManager);
                await AddUser(userManager);


            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task AddUser(UserManager<AppUser> _userManager)
        {
            var sysuser = new AppUser
            {
                Email = "sysadmin@inlandeed.co.id",
                UserName = "sysadmin@inlandeed.co.id",
                FirstName = "System Administrator",
                EmailConfirmed = true,
                Active = true
            };

            var superUserInDb = await _userManager.FindByEmailAsync(sysuser.Email);
            if (superUserInDb == null)
            {
                var rs = await _userManager.CreateAsync(sysuser, DefaultConstants.DefaultPassword);
                if (!rs.Succeeded)
                {
                    foreach (var error in rs.Errors)
                    {
                        _logger.LogError(error.Description);
                    }
                }
                var result = await _userManager.AddToRoleAsync(sysuser, DefaultRole.SystemAdmin);
                result = await _userManager.AddToRoleAsync(sysuser, DefaultRole.User);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Seeded Default User.");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError(error.Description);
                    }
                }
            }
        }
        private async Task<AppRole> AddRole(RoleManager<AppRole> _roleManager,
            string rolename, CancellationToken cancellationToken = default)
        {
            var role = new AppRole
            {
                Name = rolename,
            };
            var roleinDb = await _roleManager.FindByNameAsync(rolename);
            if (roleinDb == null)
            {
                await _roleManager.CreateAsync(role);
            }
            _logger.LogInformation($"Success Seed Role {rolename}");
            return role;
        }
        private static async Task SeedRoleSystemAdmin(RoleManager<AppRole> _roleManager)
        {
            var adminRole = await _roleManager.FindByNameAsync(DefaultRole.SystemAdmin);
            if (adminRole == null)
            {
                var role = new AppRole
                {
                    Name = DefaultRole.SystemAdmin,
                };
                await _roleManager.CreateAsync(role);
                adminRole = role;
            }
            var allClaims = await _roleManager.GetClaimsAsync(adminRole);

            var defaultpermissions = new List<string>()
            {
                DefaultPermission.Create,
                DefaultPermission.View,
                DefaultPermission.Edit,
                DefaultPermission.Delete
            };
            foreach (var permission in defaultpermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await _roleManager.AddClaimAsync(adminRole, new Claim("Permission", permission));
                }
            }
        }
        private static async Task SeedRoleBasicUser(RoleManager<AppRole> _roleManager)
        {
            var adminRole = await _roleManager.FindByNameAsync(DefaultRole.User);
            if (adminRole == null)
            {
                var role = new AppRole
                {
                    Name = DefaultRole.User,
                    //Description = DefaultRole.TenantAdmin,
                    //TenantId = tenant.Id,
                    //IsActive = true
                };
                await _roleManager.CreateAsync(role);
                adminRole = role;
            }
            var allClaims = await _roleManager.GetClaimsAsync(adminRole);

            var defaultpermissions = new List<string>()
            {
                //DefaultPermission.Create,
                DefaultPermission.View,
                //DefaultPermission.Edit
            };
            foreach (var permission in defaultpermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await _roleManager.AddClaimAsync(adminRole, new Claim("Permission", permission));
                }
            }
        }
        private static async Task SeedRoleMerchant(RoleManager<AppRole> _roleManager)
        {
            var adminRole = await _roleManager.FindByNameAsync(DefaultRole.Merchant);
            if (adminRole == null)
            {
                var role = new AppRole
                {
                    Name = DefaultRole.Merchant,
                    //Description = DefaultRole.TenantAdmin,
                    //TenantId = tenant.Id,
                    //IsActive = true
                };
                await _roleManager.CreateAsync(role);
                adminRole = role;
            }
            var allClaims = await _roleManager.GetClaimsAsync(adminRole);

            var defaultpermissions = new List<string>()
            {
                DefaultPermission.Create,
                DefaultPermission.View,
                DefaultPermission.Edit
            };
            foreach (var permission in defaultpermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await _roleManager.AddClaimAsync(adminRole, new Claim("Permission", permission));
                }
            }
        }
    }

    internal class DefaultPermission
    {
        internal static readonly string View = "View";
        internal static readonly string Edit = "Edit";
        internal static readonly string Delete = "Delete";
        internal static readonly string Create = "Create";
    }

    internal class DefaultRole
    {
        internal static readonly string SystemAdmin = "SystemAdministrator";
        internal static readonly string User = "User";
        internal static readonly string Merchant = "Merchant";
        //internal static readonly string TenantAdmin = "TenantAdministrator";
    }
    public static class DefaultConstants
    {
        public const string SysAdminRole = "SystemAdministrator";
        public const string SysAdminDesc = "System Administrator";
        public const string BasicRole = "Basic";
        public const string UserRole = "User";
        public const string DefaultPassword = "P@ssw0rd";
    }
}
