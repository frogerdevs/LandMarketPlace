namespace IdentityServer.Features.Provincies.Queries
{
    public class GetProvinciesQuery : IQuery<BaseWithDataCountResponse>
    {
    }

    public sealed class GetProvinciesQueryHandler : IQueryHandler<GetProvinciesQuery, BaseWithDataCountResponse>
    {
        private readonly IBaseRepositoryAsync<Province, string> _provinceRepo;
        public GetProvinciesQueryHandler(IBaseRepositoryAsync<Province, string> provinceRepo)
        {
            _provinceRepo = provinceRepo;
        }

        public async ValueTask<BaseWithDataCountResponse> Handle(GetProvinciesQuery query, CancellationToken cancellationToken)
        {
            var provincies = await _provinceRepo.GetAllAsync(cancellationToken);

            var res = new BaseWithDataCountResponse
            {
                Success = true,
                Message = "Success Get Data",
                Count = provincies.Count,
                Data = provincies
            };
            return res;
        }

    }
}
