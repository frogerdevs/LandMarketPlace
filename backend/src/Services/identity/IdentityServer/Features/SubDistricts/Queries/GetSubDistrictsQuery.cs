namespace IdentityServer.Features.SubDistricts.Queries
{
    public class GetSubDistrictsQuery : IQuery<BaseWithDataCountResponse>
    {
    }

    public sealed class GetSubDistrictsQueryHandler : IQueryHandler<GetSubDistrictsQuery, BaseWithDataCountResponse>
    {
        private readonly IBaseRepositoryAsync<SubDistrict, string> _subdistrictRepo;
        public GetSubDistrictsQueryHandler(IBaseRepositoryAsync<SubDistrict, string> repo)
        {
            _subdistrictRepo = repo;
        }

        public async ValueTask<BaseWithDataCountResponse> Handle(GetSubDistrictsQuery query, CancellationToken cancellationToken)
        {
            var subdistricts = await _subdistrictRepo.GetAllAsync(cancellationToken);

            var res = new BaseWithDataCountResponse
            {
                Success = true,
                Message = "Success Get Data",
                Count = subdistricts.Count,
                Data = subdistricts
            };
            return res;
        }

    }
}
