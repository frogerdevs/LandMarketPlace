namespace IdentityServer.Features.Profile.Queries
{
    public class GetContactPersonMerchantQuery : IQuery<ContactPersonResponse?>
    {
        public required string UserId { get; set; }
    }

    public sealed class GetContactPersonMerchantQueryHandler : IQueryHandler<GetContactPersonMerchantQuery, ContactPersonResponse?>
    {
        private readonly UserManager<AppUser> _userManager;
        public GetContactPersonMerchantQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async ValueTask<ContactPersonResponse?> Handle(GetContactPersonMerchantQuery query, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                  .Select(c => new ContactPersonResponse
                  {
                      UserId = c.Id,
                      Email = c.Email,
                      Contact = c.SellerProfile == null ? null : c.SellerProfile.Contact,
                      WhatsApp = c.SellerProfile == null ? null : c.SellerProfile.WhatsApp,
                      Facebook = c.SellerProfile == null ? null : c.SellerProfile.Facebook,
                      Instagram = c.SellerProfile == null ? null : c.SellerProfile.Instagram,
                      Twitter = c.SellerProfile == null ? null : c.SellerProfile.Twitter,
                      Tiktok = c.SellerProfile == null ? null : c.SellerProfile.Tiktok,
                      Website = c.SellerProfile == null ? null : c.SellerProfile.Website,
                      BrandName = c.SellerProfile == null ? null : c.SellerProfile.BrandName,
                      BrandSlug = c.SellerProfile == null ? null : c.SellerProfile.SlugBrand,
                      ImageUrl = c.ImageUrl,
                  }).AsNoTracking()
                  .FirstOrDefaultAsync(c => c.UserId == query.UserId, cancellationToken);
            return user;
        }
    }

}
