using IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

namespace IdentityServer.Features.Users.Commands
{
    public partial class ForgotPasswordCommand : ICommand<BaseResponse?>
    {
        public string Email { get; set; }
        public string HostUrl { get; set; }
    }
    public sealed class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand, BaseResponse?>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserStore<AppUser> _userStore;
        private readonly IEmailService _mailService;
        public ForgotPasswordCommandHandler(UserManager<AppUser> userManager,
            IUserStore<AppUser> userStore,
            IEmailService mailService)
        {
            _userManager = userManager;
            _userStore = userStore;
            _mailService = mailService;
        }

        public async ValueTask<BaseResponse?> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
        {
            var existuser = await _userManager.FindByEmailAsync(command.Email);
            if (existuser == null)
            {
                return null;
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(existuser);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var host = string.IsNullOrEmpty(command.HostUrl) ? "http://localhost:3000" : command.HostUrl.Replace("/api", "");
            var callbackUrl = $"{host}/auth/resetpassword?code={code}&email={command.Email}";
            await _mailService.SendEmailAsync(new Models.MailRequest
            {
                ToEmail = command.Email,
                Subject = "Reset Password",
                Body = $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
            }, cancellationToken);

            return new BaseResponse
            {
                Success = true,
                Message = "Berhasil mengirim email",
            };
        }
    }
}
