namespace IdentityServer.Features.Cities.Queries
{
    public class GetCityByProvinceIdQuery : IQuery<BaseWithDataResponse>
    {
        public required string ProvinceId { get; set; }
    }
    public sealed class GetCityByProvinceIdQueryHandler : IQueryHandler<GetCityByProvinceIdQuery, BaseWithDataResponse>
    {
        private readonly IBaseRepositoryAsync<City, string> _repoCity;

        public GetCityByProvinceIdQueryHandler(IBaseRepositoryAsync<City, string> repo)
        {
            _repoCity = repo;
        }

        public async ValueTask<BaseWithDataResponse> Handle(GetCityByProvinceIdQuery query, CancellationToken cancellationToken)
        {
            var res = new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Get Data",
            };

            var items = await _repoCity.Entities.Where(c => c.ProvinceId == query.ProvinceId).Select(c => new { c.Id, c.Name, c.ProvinceId }).ToListAsync(cancellationToken);

            if (items == null)
            {
                return null!;
            }
            res.Data = items;

            return res;
        }

    }
}
