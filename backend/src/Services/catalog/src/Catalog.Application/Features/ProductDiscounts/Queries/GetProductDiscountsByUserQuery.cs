namespace Catalog.Application.Features.ProductDiscounts.Queries
{
    public class GetProductDiscountsByUserQuery : IQuery<BaseWithDataCountResponse>
    {
        public required string UserId { get; set; }
    }
    public sealed class GetProductDiscountsByUserQueryHandler : IQueryHandler<GetProductDiscountsByUserQuery, BaseWithDataCountResponse>
    {
        private readonly IBaseRepositoryAsync<ProductDiscount, string> _repo;

        public GetProductDiscountsByUserQueryHandler(IBaseRepositoryAsync<ProductDiscount, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<BaseWithDataCountResponse> Handle(GetProductDiscountsByUserQuery query, CancellationToken cancellationToken)
        {
            var res = new BaseWithDataCountResponse
            {
                Success = true,
                Message = "Success Get Data",
            };

            var items = await _repo.Entities.Include(c => c.Product)
                .Where(c => c.UserId == query.UserId)
                .AsNoTracking().Select(i => new
                {
                    Id = i.Id,
                    i.ProductId,
                    ImageUrl = i.Product == null || i.Product.ProductImages == null || i.Product.ProductImages.FirstOrDefault() == null ? "" : i.Product.ProductImages.FirstOrDefault()!.ImageUrl,
                    i.Product!.Title,
                    i.DiscountPercent,
                    i.DiscountPrice,
                    Active = i.Active,
                }).ToListAsync(cancellationToken);
            res.Count = items.Count;
            res.Data = items;

            return res;
        }
    }

}
