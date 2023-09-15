namespace IdentityServer.Features.Provincies.Queries
{
    public class GetProvinceByIdQuery : IQuery<ProvinceItem?>
    {
        public required string Id { get; set; }
    }
    public sealed class GetProvinceByIdQueryHandler : IQueryHandler<GetProvinceByIdQuery, ProvinceItem?>
    {
        private readonly IBaseRepositoryAsync<Province, string> _repoProvince;

        public GetProvinceByIdQueryHandler(IBaseRepositoryAsync<Province, string> repo)
        {
            _repoProvince = repo;
        }

        public async ValueTask<ProvinceItem?> Handle(GetProvinceByIdQuery query, CancellationToken cancellationToken)
        {
            var items = await _repoProvince.GetByIdAsync(query.Id, cancellationToken);

            var res = items != null ? new ProvinceItem
            {
                Id = items.Id,
                Name = items.Name,
            } : null;
            return res;
        }

    }
}
