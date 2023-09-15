namespace IdentityServer.Features.Profile.Queries
{
    public class GetProfileMerchantByEmailQuery : IQuery<BaseWithDataResponse?>
    {
        public required string Email { get; set; }
    }

    public sealed class GetProfileMerchantByEmailQueryHandler : IQueryHandler<GetProfileMerchantByEmailQuery, BaseWithDataResponse?>
    {
        private readonly UserManager<AppUser> _userManager;
        public GetProfileMerchantByEmailQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async ValueTask<BaseWithDataResponse?> Handle(GetProfileMerchantByEmailQuery query, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Include(sp => sp.SellerProfile)
                  .Select(c => new
                  {
                      IsSeller = c.IsSeller,
                      c.IsCompany,
                      c.CompanyName,
                      UserName = c.UserName,
                      Email = c.Email,
                      EmailConfirmed = c.EmailConfirmed,
                      PhoneNumber = c.PhoneNumber,
                      PhoneNumberConfirmed = c.PhoneNumberConfirmed,
                      Active = c.Active,
                      CategoryId = c.SellerCategoryId,
                      c.SellerProfile.BrandName,
                      NewsLetter = c.NewsLetter,
                      c.FirstName,
                      c.LastName,
                      c.ImageUrl,
                      c.Address,
                      c.Province,
                      c.City,
                      c.District,
                      c.SubDistrict,
                      c.PostCode,
                      c.WillingContacted,
                  }).AsNoTracking()
                  .FirstOrDefaultAsync(c => c.Email == query.Email, cancellationToken);

            if (user == null)
            {
                return null;
            }
            var res = new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Get Data",
                Data = user
            };
            return res;
        }
    }
}
