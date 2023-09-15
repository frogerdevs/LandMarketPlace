namespace IdentityServer.Features.Profile.Commands
{
    public class ChangePasswordCommand : ICommand<BaseResponse?>
    {
        public required string Email { get; set; }
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
    }
    public sealed class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, BaseResponse?>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public ChangePasswordCommandHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async ValueTask<BaseResponse?> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user == null)
            {
                return null;
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, command.OldPassword, command.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                if (changePasswordResult.Errors.Any())
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = string.Join(", ", changePasswordResult.Errors.Select(e => e.Description)),
                    };
                }
            }

            await _signInManager.RefreshSignInAsync(user);

            return new BaseResponse
            {
                Success = true,
                Message = "Password berhasil dirubah",

            };

        }

    }

}
