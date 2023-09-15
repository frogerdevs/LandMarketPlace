namespace Catalog.Application.Features.Adsenses.Queries
{
    public class GetAdsenseByIdQuery : IQuery<BaseWithDataResponse>
    {
        public required string Id { get; set; }
    }
    public sealed class GetAdsenseByIdQueryHandler : IQueryHandler<GetAdsenseByIdQuery, BaseWithDataResponse>
    {
        private readonly IBaseRepositoryAsync<Adsense, string> _repo;

        public GetAdsenseByIdQueryHandler(IBaseRepositoryAsync<Adsense, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<BaseWithDataResponse> Handle(GetAdsenseByIdQuery query, CancellationToken cancellationToken)
        {
            var res = new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Get Data",
            };

            var adsense = await _repo.Entities.Include(p => p.Product).ThenInclude(pi => pi.ProductImages)
                .Select(c => new
                {
                    c.Id,
                    c.ProductId,
                    ProductName = c.Product.Title,
                    c.ImageUrl,
                    c.Title,
                    c.Slug,
                    c.Content,
                    c.Active
                })
                .FirstOrDefaultAsync(c => c.Id == query.Id, cancellationToken: cancellationToken);
            if (adsense == null)
            {
                return null!;
            }
            res.Data = adsense;

            return res;
        }

    }

}
