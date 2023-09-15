namespace Catalog.Application.Features.Adsenses.Queries
{
    public class GetAdsensesQuery : IQuery<BaseWithDataCountResponse>
    {
    }
    public sealed class GetAdsensesQueryHandler : IQueryHandler<GetAdsensesQuery, BaseWithDataCountResponse>
    {
        private readonly IBaseRepositoryAsync<Adsense, string> _repository;
        public GetAdsensesQueryHandler(IBaseRepositoryAsync<Adsense, string> repository)
        {
            _repository = repository;
        }

        public async ValueTask<BaseWithDataCountResponse> Handle(GetAdsensesQuery query, CancellationToken cancellationToken)
        {
            var items = await _repository.Entities.AsNoTracking()
                            .Select(c => new
                            {
                                c.Id,
                                c.ProductId,
                                c.ImageUrl,
                                c.Title,
                                c.Slug,
                                c.Content,
                                c.Active
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
