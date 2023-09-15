namespace IdentityServer.Features.Profile.Commands
{
    public partial class EditProfileMerchantCommand : ICommand<BaseWithDataResponse?>
    {
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string? SellerCategoryId { get; set; }
        public string? BrandName { get; set; }
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
    public sealed class EditProfileMerchantCommandHandler : ICommandHandler<EditProfileMerchantCommand, BaseWithDataResponse?>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IBaseRepositoryAsync<UserProfile, string> _sellerProfileRepo;

        public EditProfileMerchantCommandHandler(UserManager<AppUser> userManager, IBaseRepositoryAsync<UserProfile, string> sellerProfileRepo)
        {
            _userManager = userManager;
            _sellerProfileRepo = sellerProfileRepo;
        }

        public async ValueTask<BaseWithDataResponse?> Handle(EditProfileMerchantCommand command, CancellationToken cancellationToken)
        {
            var existuser = await _userManager.Users.Include(sp => sp.SellerProfile)
                .FirstOrDefaultAsync(c => c.Email == command.Email, cancellationToken: cancellationToken);
            if (existuser == null)
            {
                return null;
            }
            existuser.IsSeller = true;
            existuser.SellerCategoryId = command.SellerCategoryId;
            existuser.PhoneNumber = command.PhoneNumber;
            existuser.PhoneNumberConfirmed = command.PhoneNumberConfirmed;
            if (existuser.SellerProfile is null)
            {
                existuser.SellerProfile = new UserProfile
                {
                    BrandName = command.BrandName,
                };
            }
            else
            {
                existuser.SellerProfile.BrandName = command.BrandName;
            }
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
            existuser.WillingContacted = command.WillingContacted;

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
