namespace IdentityServer.Features.Districts.Queries
{
    public class GetDistrictByCityIdQuery : IQuery<BaseWithDataResponse>
    {
        public required string CityId { get; set; }
    }
    public sealed class GetDistrictByCityQueryHandler : IQueryHandler<GetDistrictByCityIdQuery, BaseWithDataResponse>
    {
        private readonly IBaseRepositoryAsync<District, string> _repo;

        public GetDistrictByCityQueryHandler(IBaseRepositoryAsync<District, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<BaseWithDataResponse> Handle(GetDistrictByCityIdQuery query, CancellationToken cancellationToken)
        {
            var res = new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Get Data",
            };

            var items = await _repo.Entities.Where(c => c.CityId == query.CityId).Select(c => new { c.Id, c.Name, c.CityId }).ToListAsync(cancellationToken);

            if (items == null)
            {
                return null!;
            }
            res.Data = items;

            return res;
        }

    }
}
