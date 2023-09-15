namespace IdentityServer.Features.Profile.Commands
{
    public partial class EditProfileLandersCommand : ICommand<BaseWithDataResponse?>
    {
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string? ImageUrl { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool NewsLetter { get; set; }
    }
    public sealed class EditProfileLandersCommandHandler : ICommandHandler<EditProfileLandersCommand, BaseWithDataResponse?>
    {
        private readonly UserManager<AppUser> _userManager;

        public EditProfileLandersCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async ValueTask<BaseWithDataResponse?> Handle(EditProfileLandersCommand command, CancellationToken cancellationToken)
        {
            var existuser = await _userManager.FindByEmailAsync(command.Email);
            if (existuser == null)
            {
                return null;
            }
            existuser.PhoneNumber = command.PhoneNumber;
            existuser.ImageUrl = command.ImageUrl;
            existuser.FirstName = command.FirstName;
            existuser.LastName = command.LastName;
            existuser.NewsLetter = command.NewsLetter;
            existuser.PhoneNumberConfirmed = command.PhoneNumberConfirmed;

            var identityResult = await _userManager.UpdateAsync(existuser);

            if (identityResult.Errors.Any())
            {
                return new BaseWithDataResponse
                {
                    Success = false,
                    Message = string.Join(", ", identityResult.Errors.Select(e => e.Description)),
                    Data = null
                };
            }

            return new BaseWithDataResponse
            {
                Success = true,
                Message = "Berhasil",
                Data = new
                {
                    Email = existuser.Email,
                    EmailConfirmed = existuser.EmailConfirmed,
                    PhoneNumber = existuser.PhoneNumber,
                    PhoneNumberConfirmed = existuser.PhoneNumberConfirmed
                }
            };
        }

    }

}
