using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace IdentityServer.Features.Users.Commands
{

    public partial class ResetPasswordCommand : ICommand<BaseResponse?>
    {
        public string Code { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public sealed class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, BaseResponse?>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserStore<AppUser> _userStore;

        public ResetPasswordCommandHandler(UserManager<AppUser> userManager,
            IUserStore<AppUser> userStore)
        {
            _userManager = userManager;
            _userStore = userStore;

        }

        public async ValueTask<BaseResponse?> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            var existuser = await _userManager.FindByEmailAsync(command.Email);
            if (existuser == null)
            {
                return null;
            }
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(command.Code));
            var result = await _userManager.ResetPasswordAsync(existuser, code, command.Password);
            if (!result.Succeeded)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Gagal reset password",
                };
            }

            return new BaseResponse
            {
                Success = true,
                Message = "Berhasil reset password",
            };
        }
    }
}
