namespace IdentityServer.Features.Users.Queries
{
    public class GetUserByIdQuery : IQuery<BaseWithDataResponse<UserItemResponse>?>
    {
        public required string Id { get; set; }
    }
    public sealed class GetUserByIdQueryhandler : IQueryHandler<GetUserByIdQuery, BaseWithDataResponse<UserItemResponse>?>
    {
        private readonly UserManager<AppUser> _userManager;
        public GetUserByIdQueryhandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async ValueTask<BaseWithDataResponse<UserItemResponse>?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Include(c => c.SellerProfile)
                .Select(user => new UserItemResponse
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumber = user.PhoneNumber,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    Active = user.Active,
                    IsCompany = (user.SellerProfile != null) && user.SellerProfile.IsCompany,
                    BrandName = (user.SellerProfile != null) ? user.SellerProfile.BrandName : "",
                    BrandSlug = (user.SellerProfile != null) ? user.SellerProfile.SlugBrand : "",

                })
                .FirstOrDefaultAsync(c => c.Id == query.Id, cancellationToken);

            if (user == null)
            {
                return null;
            }
            //var item = MapToUserItem(user);
            return new BaseWithDataResponse<UserItemResponse>
            {
                Success = true,
                Message = "Berhasil Get Data",
                Data = user,
            };
        }

        private UserItemResponse MapToUserItem(AppUser? user)
        {
            return new UserItemResponse
            {
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                Active = user.Active,
                IsCompany = (user.SellerProfile is not null) && user.SellerProfile.IsCompany,
                BrandName = user.SellerProfile?.BrandName,
            };
        }
    }
}
