namespace IdentityServer.Features.SubDistricts.Queries
{
    public class GetSubDistrictByDistrictIdQuery : IQuery<BaseWithDataResponse>
    {
        public required string DistrictId { get; set; }
    }
    public sealed class GetSubDistrictByDistrictIdQueryHandler : IQueryHandler<GetSubDistrictByDistrictIdQuery, BaseWithDataResponse>
    {
        private readonly IBaseRepositoryAsync<SubDistrict, string> _repo;

        public GetSubDistrictByDistrictIdQueryHandler(IBaseRepositoryAsync<SubDistrict, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<BaseWithDataResponse> Handle(GetSubDistrictByDistrictIdQuery query, CancellationToken cancellationToken)
        {
            var res = new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Get Data",
            };

            var items = await _repo.Entities.Where(c => c.DistrictId == query.DistrictId).ToListAsync(cancellationToken);

            if (items == null)
            {
                return null!;
            }
            res.Data = items;

            return res;
        }

    }
}
