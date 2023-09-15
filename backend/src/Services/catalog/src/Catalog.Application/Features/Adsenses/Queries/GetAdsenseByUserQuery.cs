namespace Catalog.Application.Features.Adsenses.Queries
{
    public class GetAdsenseByUserQuery : IQuery<BaseWithDataCountResponse>
    {
        public required string UserId { get; set; }
    }
    public sealed class GetAdsenseByUserQueryHandler : IQueryHandler<GetAdsenseByUserQuery, BaseWithDataCountResponse>
    {
        private readonly IBaseRepositoryAsync<Adsense, string> _repo;

        public GetAdsenseByUserQueryHandler(IBaseRepositoryAsync<Adsense, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<BaseWithDataCountResponse> Handle(GetAdsenseByUserQuery query, CancellationToken cancellationToken)
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
                    ProductName = i.Product!.Title,
                    i.ImageUrl,
                    i.Title,
                    Active = i.Active,
                    i.Slug,
                    i.Content,
                    i.StartFrom,
                    i.StartTo,
                }).ToListAsync(cancellationToken);
            res.Count = items.Count;
            res.Data = items;

            return res;
        }
    }
}
