namespace IdentityServer.Features.Users.Queries
{
    public class GetMerchantByCategorySlugQuery : IQuery<BasePagingResponse<MerchantByCategoryItem>>
    {
        public required string CategoryId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    public sealed class GetMerchantByCategorySlugQueryHandler : IQueryHandler<GetMerchantByCategorySlugQuery, BasePagingResponse<MerchantByCategoryItem>>
    {
        private readonly UserManager<AppUser> _userManager;

        public GetMerchantByCategorySlugQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async ValueTask<BasePagingResponse<MerchantByCategoryItem>> Handle(GetMerchantByCategorySlugQuery query, CancellationToken cancellationToken)
        {
            var response = new BasePagingResponse<MerchantByCategoryItem>
            {
                Limit = query.PageSize,
                CurrentPage = query.PageNumber,
                Success = false,
                Message = "Failed Get Data",
                TotalData = 0,
                Count = 0,
                Data = null
            };

            var queryUser = _userManager.Users
                .Select(c => new MerchantByCategoryItem
                {
                    Id = c.Id,
                    BrandSlug = c.SellerProfile!.SlugBrand,
                    Province = c.Province,
                    City = c.City,
                    District = c.District,
                    ImageUrl = c.ImageUrl,
                    Active = c.Active,
                    BrandName = (c.SellerProfile != null) ? c.SellerProfile.BrandName : null,
                    CategoryId = c.SellerCategoryId,
                    IsSeller = c.IsSeller,
                })
                .Where(c => c.Active && c.IsSeller && c.CategoryId == query.CategoryId)
                .AsNoTracking();
            var totaldata = await queryUser.CountAsync(cancellationToken: cancellationToken);

            var users = await queryUser
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            if (users != null)
            {
                response.TotalData = totaldata;
                response.Count = users.Count;
                response.Data = users;
                response.Success = true;
                response.Message = "Success Get Data";

                return response;
            }
            return response;
        }
    }

}
