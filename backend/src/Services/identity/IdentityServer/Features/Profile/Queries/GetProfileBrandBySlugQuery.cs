namespace IdentityServer.Features.Profile.Queries
{
    public class GetProfileBrandBySlugQuery : IQuery<ProfileMerchantResponse?>
    {
        public required string Slug { get; set; }
    }

    public sealed class GetProfileBrandBySlugQueryHandler : IQueryHandler<GetProfileBrandBySlugQuery, ProfileMerchantResponse?>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IBaseRepositoryAsync<Province, string> _repoProvince;
        private readonly IBaseRepositoryAsync<City, string> _repoCity;
        private readonly IBaseRepositoryAsync<District, string> _repoDistrict;
        private readonly IBaseRepositoryAsync<SubDistrict, string> _repoSubDistrict;
        public GetProfileBrandBySlugQueryHandler(UserManager<AppUser> userManager,
            IBaseRepositoryAsync<Province, string> repoProvince,
            IBaseRepositoryAsync<City, string> repoCity,
            IBaseRepositoryAsync<District, string> repoDistrict,
            IBaseRepositoryAsync<SubDistrict, string> repoSubDistrict)
        {
            _userManager = userManager;
            _repoProvince = repoProvince;
            _repoCity = repoCity;
            _repoDistrict = repoDistrict;
            _repoSubDistrict = repoSubDistrict;
        }
        public async ValueTask<ProfileMerchantResponse?> Handle(GetProfileBrandBySlugQuery query, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                  .Select(c => new ProfileMerchantResponse
                  {
                      UserId = c.Id,
                      BrandName = c.SellerProfile == null ? null : c.SellerProfile.BrandName,
                      BrandSlug = c.SellerProfile == null ? null : c.SellerProfile.SlugBrand,
                      Address = c.Address,
                      Province = _repoProvince.Entities.AsNoTracking().FirstOrDefault()!.Name,
                      City = _repoCity.Entities.AsNoTracking().FirstOrDefault()!.Name,
                      District = _repoDistrict.Entities.AsNoTracking().FirstOrDefault()!.Name,
                      SubDistrict = _repoSubDistrict.Entities.AsNoTracking().FirstOrDefault()!.Name,
                      PostCode = c.PostCode,
                      ImageUrl = c.ImageUrl,
                      Verified = c.Verified,
                      CategoryId = c.SellerCategoryId
                  }).AsNoTracking()
                  .FirstOrDefaultAsync(c => c.BrandSlug == query.Slug, cancellationToken);
            return user;
        }
    }
}
