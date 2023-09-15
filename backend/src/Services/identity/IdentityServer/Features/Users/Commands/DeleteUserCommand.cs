using IdentityServer.Data.Entites;
using IdentityServer.Dtos.Responses.Base;
using Mediator;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Features.Users.Commands
{
    public class DeleteUserCommand : IRequest<BaseResponse?>
    {
        public required string Email { get; set; }
    }
    public sealed class DeleteUserCommandhandler : IRequestHandler<DeleteUserCommand, BaseResponse?>
    {
        private readonly UserManager<AppUser> _userManager;

        public DeleteUserCommandhandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async ValueTask<BaseResponse?> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            var existuser = await _userManager.FindByEmailAsync(command.Email);
            if (existuser == null)
            {
                return null;
            }

            var identityResult = await _userManager.DeleteAsync(existuser);
            if (identityResult.Errors.Any())
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = string.Join(", ", identityResult.Errors.Select(e => e.Description)),
                };
            }

            return new BaseResponse
            {
                Success = true,
                Message = "User Berhasil Dihapus."
            };
        }
    }

}
