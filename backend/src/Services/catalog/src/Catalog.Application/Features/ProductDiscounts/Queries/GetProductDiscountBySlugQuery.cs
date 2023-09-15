namespace Catalog.Application.Features.ProductDiscounts.Queries
{
    public class GetProductDiscountBySlugQuery : IQuery<BaseWithDataResponse>
    {
        public required string Slug { get; set; }
    }
    public sealed class GetProductDiscountBySlugQueryHandler : IQueryHandler<GetProductDiscountBySlugQuery, BaseWithDataResponse>
    {
        private readonly IBaseRepositoryAsync<ProductDiscount, string> _repo;

        public GetProductDiscountBySlugQueryHandler(IBaseRepositoryAsync<ProductDiscount, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<BaseWithDataResponse> Handle(GetProductDiscountBySlugQuery query, CancellationToken cancellationToken)
        {
            var res = new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Get Data",
            };

            var items = await _repo.Entities.AsNoTracking().FirstOrDefaultAsync(c => c.Slug == query.Slug, cancellationToken);

            if (items == null)
            {
                return null!;
            }
            res.Data = items;

            return res;
        }

    }

}
