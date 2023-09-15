namespace Catalog.Application.Features.Adsenses.Queries
{
    public class GetAdsenseBySlugQuery : IQuery<BaseWithDataResponse>
    {
        public required string Slug { get; set; }
    }
    public sealed class GetAdsenseBySlugQueryHandler : IQueryHandler<GetAdsenseBySlugQuery, BaseWithDataResponse>
    {
        private readonly IBaseRepositoryAsync<Adsense, string> _repo;

        public GetAdsenseBySlugQueryHandler(IBaseRepositoryAsync<Adsense, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<BaseWithDataResponse> Handle(GetAdsenseBySlugQuery query, CancellationToken cancellationToken)
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
