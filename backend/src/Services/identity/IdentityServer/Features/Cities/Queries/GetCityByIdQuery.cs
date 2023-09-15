namespace IdentityServer.Features.Cities.Queries
{
    public class GetCityByIdQuery : IQuery<CityItem?>
    {
        public required string Id { get; set; }
    }
    public sealed class GetCityByIdQueryHandler : IQueryHandler<GetCityByIdQuery, CityItem?>
    {
        private readonly IBaseRepositoryAsync<City, string> _repoCity;

        public GetCityByIdQueryHandler(IBaseRepositoryAsync<City, string> repo)
        {
            _repoCity = repo;
        }

        public async ValueTask<CityItem?> Handle(GetCityByIdQuery query, CancellationToken cancellationToken)
        {

            var items = await _repoCity.GetByIdAsync(query.Id, cancellationToken);

            var res = items != null ? new CityItem
            {
                Id = items.Id,
                Name = items.Name,
            } : null;
            return res;
        }
    }
}
