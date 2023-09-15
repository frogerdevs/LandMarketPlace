using IdentityServer.Data.Entites;
using IdentityServer.Dtos.Responses.Base;
using IdentityServer.Dtos.Responses.Users;
using Mediator;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Features.Users.Commands
{
    public partial class EditUserCommand : ICommand<BaseWithDataResponse?>
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
    public sealed class EditUserCommandHandler : ICommandHandler<EditUserCommand, BaseWithDataResponse?>
    {
        private readonly UserManager<AppUser> _userManager;

        public EditUserCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async ValueTask<BaseWithDataResponse?> Handle(EditUserCommand command, CancellationToken cancellationToken)
        {
            var existuser = await _userManager.FindByEmailAsync(command.Email);
            if (existuser == null)
            {
                return null;
            }
            existuser.Email = command.Email;
            existuser.PhoneNumber = command.PhoneNumber;
            existuser.ImageUrl = command.ImageUrl;
            existuser.FirstName = command.FirstName;
            existuser.LastName = command.LastName;
            existuser.CompanyName = command.CompanyName;
            existuser.Address = command.Address;
            existuser.Country = command.Country;
            existuser.Province = command.Province;
            existuser.City = command.City;
            existuser.District = command.District;
            existuser.SubDistrict = command.SubDistrict;
            existuser.PostCode = command.PostCode;
            existuser.NewsLetter = command.NewsLetter;
            existuser.WillingContacted = command.WillingContacted;
            existuser.Active = true;

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
            var result = MapToItemResponse(existuser);
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

    }

}
