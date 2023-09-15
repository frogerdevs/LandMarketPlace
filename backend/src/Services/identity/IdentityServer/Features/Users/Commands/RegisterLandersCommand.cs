using Humanizer;
using IdentityServer.Configs;
using IdentityServer.Data.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace IdentityServer.Features.Users.Commands
{
    public partial class RegisterLandersCommand : RegisterLandersRequest, ICommand<BaseResponse?>
    {
    }
    public sealed class RegisterLandersCommandHandler : ICommandHandler<RegisterLandersCommand, BaseResponse?>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserStore<AppUser> _userStore;
        private readonly AuthSettings _authSettings;
        public RegisterLandersCommandHandler(UserManager<AppUser> userManager,
            IUserStore<AppUser> userStore, IOptions<AuthSettings> authSettings)
        {
            _userManager = userManager;
            _userStore = userStore;
            _authSettings = authSettings.Value;
        }

        public async ValueTask<BaseResponse?> Handle(RegisterLandersCommand command, CancellationToken cancellationToken)
        {
            var existuser = await _userManager.FindByEmailAsync(command.Email);
            if (existuser != null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Email sudah di gunakan."
                };
            }
            var user = MapToAppUser(command);
            await _userStore.SetUserNameAsync(user, command.Email, CancellationToken.None);
            IdentityResult identityResult = new();
            if (string.IsNullOrEmpty(command.Provider))
            {
                identityResult = await _userManager.CreateAsync(user, command.Password!);
                if (identityResult.Succeeded)
                {
                    identityResult = await _userManager.AddToRoleAsync(user, DefaultRole.User);
                    if (identityResult.Succeeded)
                    {
                        return new BaseResponse
                        {
                            Success = true,
                            Message = "Berhasil",
                        };
                    }
                }
            }
            else
            {
                identityResult = await _userManager.CreateAsync(user);
                if (identityResult.Succeeded)
                {
                    identityResult = await _userManager.AddToRoleAsync(user, DefaultRole.User);
                    if (identityResult.Succeeded)
                    {
                        UserLoginInfo info = new(command.Provider.ToLower(), _authSettings.Google?.ClientSecret ?? "", command.Provider.Pascalize());
                        identityResult = await _userManager.AddLoginAsync(user, info);
                        if (identityResult.Succeeded)
                        {
                            return new BaseResponse
                            {
                                Success = true,
                                Message = "Berhasil",
                            };
                        }
                    }
                }
            }

            return new BaseResponse
            {
                Success = false,
                Message = string.Join(", ", identityResult.Errors.Select(e => e.Description)),
            };

        }

        private static AppUser MapToAppUser(RegisterLandersCommand command)
        {
            return new AppUser
            {
                Active = true,
                IsSeller = false,
                FirstName = command.FirstName,
                Email = command.Email,
                PhoneNumber = command.PhoneNumber,
                EmailConfirmed = command.EmailConfirmed,
            };
        }
    }

}
