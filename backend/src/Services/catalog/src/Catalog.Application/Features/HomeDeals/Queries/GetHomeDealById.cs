namespace Catalog.Application.Features.HomeDeals.Queries
{
    public class GetHomeDealById : IQuery<BaseWithDataResponse>
    {
        public required string Id { get; set; }
    }
    public sealed class GetHomeDealByIdQueryHandler : IQueryHandler<GetHomeDealById, BaseWithDataResponse>
    {
        private readonly IBaseRepositoryAsync<HomeDeal, string> _repo;

        public GetHomeDealByIdQueryHandler(IBaseRepositoryAsync<HomeDeal, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<BaseWithDataResponse> Handle(GetHomeDealById query, CancellationToken cancellationToken)
        {
            var res = new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Get Data",
            };

            var homeDeal = await _repo.Entities.Include(p => p.Product).ThenInclude(pi => pi.ProductImages)
                .Where(c => c.Id == query.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (homeDeal == null)
            {
                return null!;
            }

            var item = MapToHomeDealItemResponse(homeDeal);

            res.Data = item;

            return res;
        }

        private static HomeDealItemResponse MapToHomeDealItemResponse(HomeDeal homeDeal)
        {
            return new HomeDealItemResponse
            {
                Id = homeDeal.Id,
                ProductId = homeDeal.ProductId,
                ImgUrl = homeDeal.ImgUrl,
                Active = homeDeal.Active,
                StartDate = homeDeal.StartDate,
                EndDate = homeDeal.EndDate,
                Product = homeDeal.Product != null ? new ProductItemResponse
                {
                    Id = homeDeal.Product.Id,
                    UserId = homeDeal.Product.UserId,
                    CategoryId = homeDeal.Product.CategoryId,
                    Title = homeDeal.Product.Title,
                    Slug = homeDeal.Product.Slug,
                    PriceFrom = homeDeal.Product.PriceFrom,
                    PriceTo = homeDeal.Product.PriceTo,
                    Description = homeDeal.Product.Description,
                    Details = homeDeal.Product.Details,
                    LocationLongitude = homeDeal.Product.LocationLongitude,
                    LocationLatitude = homeDeal.Product.LocationLatitude,
                    Active = homeDeal.Product.Active,
                    ProductImages = homeDeal.Product.ProductImages?.Select(productProductProductImage => new ProductImageItemResponse
                    {
                        ProductId = productProductProductImage.ProductId,
                        ImageUrl = productProductProductImage.ImageUrl,
                        ImageType = productProductProductImage.ImageType,
                        ImageName = productProductProductImage.ImageName
                    }).ToList()
                } : null
            };
        }
    }

}
