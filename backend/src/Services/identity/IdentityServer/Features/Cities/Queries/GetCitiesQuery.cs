namespace IdentityServer.Features.Cities.Queries
{
    public class GetCitiesQuery : IQuery<BaseWithDataCountResponse>
    {
    }

    public sealed class GetCitiesQueryHandler : IQueryHandler<GetCitiesQuery, BaseWithDataCountResponse>
    {
        private readonly IBaseRepositoryAsync<City, string> _cityRepo;
        public GetCitiesQueryHandler(IBaseRepositoryAsync<City, string> repo)
        {
            _cityRepo = repo;
        }

        public async ValueTask<BaseWithDataCountResponse> Handle(GetCitiesQuery query, CancellationToken cancellationToken)
        {
            var cities = await _cityRepo.GetAllAsync(cancellationToken);

            var res = new BaseWithDataCountResponse
            {
                Success = true,
                Message = "Success Get Data",
                Count = cities.Count,
                Data = cities
            };
            return res;
        }

    }
}
