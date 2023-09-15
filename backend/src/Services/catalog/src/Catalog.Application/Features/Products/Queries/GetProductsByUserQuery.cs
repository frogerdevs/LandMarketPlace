namespace Catalog.Application.Features.Products.Queries
{
    public class GetProductsByUserQuery : IQuery<BaseWithDataCountResponse>
    {
        public required string UserId { get; set; }
    }
    public sealed class GetProductByUserQueryHandler : IQueryHandler<GetProductsByUserQuery, BaseWithDataCountResponse>
    {
        private readonly IBaseRepositoryAsync<Product, string> _repo;

        public GetProductByUserQueryHandler(IBaseRepositoryAsync<Product, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<BaseWithDataCountResponse> Handle(GetProductsByUserQuery query, CancellationToken cancellationToken)
        {
            var items = await _repo.Entities.Include(c => c.Category).Include(c => c.ProductImages)
                .Where(c => c.UserId == query.UserId)
                .AsNoTracking()
                .Select(c => new
                {
                    c.Id,
                    CategoryId = c.CategoryId,
                    CategoryName = c.Category.Name,
                    Title = c.Title,
                    c.Slug,
                    c.Address,
                    PriceFrom = c.PriceFrom,
                    PriceTo = c.PriceTo,
                    Active = c.Active,
                    ImageUrl = c.ProductImages == null || c.ProductImages.FirstOrDefault() == null ? "" : c.ProductImages.FirstOrDefault()!.ImageUrl
                }).ToListAsync(cancellationToken);

            var res = new BaseWithDataCountResponse
            {
                Success = true,
                Message = "Success Get Data",
                Count = items.Count,
                Data = items
            };
            return res;
        }
    }

}
