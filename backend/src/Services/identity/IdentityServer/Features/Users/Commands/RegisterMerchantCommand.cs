using Slugify;

namespace IdentityServer.Features.Users.Commands
{
    public partial class RegisterMerchantCommand : RegisterMerchantRequest, ICommand<BaseResponse?>
    {
    }
    public sealed class RegisterMerchantCommandHandler : ICommandHandler<RegisterMerchantCommand, BaseResponse?>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserStore<AppUser> _userStore;
        private readonly IBaseRepositoryAsync<UserProfile, string> _sellerProfileRepo;

        public RegisterMerchantCommandHandler(UserManager<AppUser> userManager,
            IUserStore<AppUser> userStore,
            IBaseRepositoryAsync<UserProfile, string> sellerProfileRepo)
        {
            _userManager = userManager;
            _userStore = userStore;
            _sellerProfileRepo = sellerProfileRepo;
        }

        public async ValueTask<BaseResponse?> Handle(RegisterMerchantCommand command, CancellationToken cancellationToken)
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
            SlugHelper helper = new();
            string slugBrand = helper.GenerateSlug(command.BrandName);
            var profile = await _sellerProfileRepo.Entities
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.BrandName == command.BrandName || c.SlugBrand == slugBrand, cancellationToken: cancellationToken);
            if (profile != null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Brand sudah di gunakan."
                };
            }

            var user = MapToAppUser(command, slugBrand);
            await _userStore.SetUserNameAsync(user, command.Email, CancellationToken.None);
            var identityResult = await _userManager.CreateAsync(user, command.Password);

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
                Message = "Berhasil",
            };
        }

        private static AppUser MapToAppUser(RegisterMerchantCommand command, string slugBrand)
        {
            return new AppUser
            {
                Active = true,
                IsSeller = true,
                SellerCategoryId = command.CategoryId,
                IsCompany = command.IsCompany,
                CompanyName = command.CompanyName,
                Address = command.Address,
                Country = "ID",
                Province = command.Province,
                City = command.City,
                District = command.District,
                SubDistrict = command.SubDistrict,
                PostCode = command.PostCode,
                Email = command.Email,
                EmailConfirmed = false,
                PhoneNumber = command.PhoneNumber,
                SellerProfile = new UserProfile
                {
                    BrandName = command.BrandName,
                    SlugBrand = slugBrand,
                    IsCompany = command.IsCompany
                }
            };
        }
    }

}
