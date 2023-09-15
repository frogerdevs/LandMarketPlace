using IdentityServer.Dtos.Responses.Users;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Features.Users.Commands
{
    public partial class AddUserCommand : ICommand<BaseWithDataResponse?>
    {
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Password { get; set; }
        public bool IsSeller { get; set; }
        public string? ImageUrl { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? Province { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? SubDistrict { get; set; }
        public string? PostCode { get; set; }
        public bool NewsLetter { get; set; }
        public bool WillingContacted { get; set; }

    }
    public sealed class AddUserCommandHandler : ICommandHandler<AddUserCommand, BaseWithDataResponse?>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserStore<AppUser> _userStore;

        public AddUserCommandHandler(UserManager<AppUser> userManager,
            IUserStore<AppUser> userStore)
        {
            _userManager = userManager;
            _userStore = userStore;

        }

        public async ValueTask<BaseWithDataResponse?> Handle(AddUserCommand command, CancellationToken cancellationToken)
        {
            var existuser = await _userManager.FindByEmailAsync(command.Email);
            if (existuser != null)
            {
                return new BaseWithDataResponse
                {
                    Success = false,
                    Message = "Email sudah di gunakan.",
                    Data = null
                };
            }
            //existuser = await _userManager.Users.FirstOrDefaultAsync(c => c.PhoneNumber == command.PhoneNumber, cancellationToken: cancellationToken); ;
            //if (existuser != null)
            //{
            //    return new BaseWithDataResponse
            //    {
            //        Success = false,
            //        Message = "Nomor Telpon sudah di gunakan.",
            //        Data = null
            //    };
            //}
            var user = MapToAppUser(command);
            await _userStore.SetUserNameAsync(user, command.Email, CancellationToken.None);
            var identityResult = await _userManager.CreateAsync(user, command.Password);

            if (identityResult.Errors.Any())
            {
                return new BaseWithDataResponse
                {
                    Success = false,
                    Message = string.Join(", ", identityResult.Errors.Select(e => e.Description)),
                    Data = null
                };
            }

            var result = MapToItemResponse(user);
            return new BaseWithDataResponse
            {
                Success = true,
                Message = "Berhasil",
                Data = result
            };
        }

        private static UserItemResponse MapToItemResponse(AppUser user)
        {
            return new UserItemResponse
            {
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                Active = user.Active
            };
        }

        private static AppUser MapToAppUser(AddUserCommand command)
        {
            return new AppUser
            {
                Email = command.Email,
                PhoneNumber = command.PhoneNumber,
                ImageUrl = command.ImageUrl,
                FirstName = command.FirstName,
                LastName = command.LastName,
                CompanyName = command.CompanyName,
                Address = command.Address,
                Country = command.Country,
                Province = command.Province,
                City = command.City,
                District = command.District,
                SubDistrict = command.SubDistrict,
                PostCode = command.PostCode,
                NewsLetter = command.NewsLetter,
                WillingContacted = command.WillingContacted,
                Active = true,
            };
        }
    }

}
