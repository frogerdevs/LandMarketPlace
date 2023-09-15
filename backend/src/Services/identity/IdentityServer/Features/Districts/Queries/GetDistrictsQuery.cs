namespace IdentityServer.Features.Districts.Queries
{
    public class GetDistrictsQuery : IQuery<BaseWithDataCountResponse>
    {
    }

    public sealed class GetDistrictQueryHandler : IQueryHandler<GetDistrictsQuery, BaseWithDataCountResponse>
    {
        private readonly IBaseRepositoryAsync<District, string> _districtRepo;
        public GetDistrictQueryHandler(IBaseRepositoryAsync<District, string> repo)
        {
            _districtRepo = repo;
        }

        public async ValueTask<BaseWithDataCountResponse> Handle(GetDistrictsQuery query, CancellationToken cancellationToken)
        {
            var districts = await _districtRepo.GetAllAsync(cancellationToken);

            var res = new BaseWithDataCountResponse
            {
                Success = true,
                Message = "Success Get Data",
                Count = districts.Count,
                Data = districts
            };
            return res;
        }

    }
}
