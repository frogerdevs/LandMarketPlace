namespace IdentityServer.Features.SubDistricts.Queries
{
    public class GetSubDistrictByIdQuery : IQuery<SubDistrictsItem?>
    {
        public required string Id { get; set; }
    }
    public sealed class GetSubDistrictByIdQueryHandler : IQueryHandler<GetSubDistrictByIdQuery, SubDistrictsItem?>
    {
        private readonly IBaseRepositoryAsync<SubDistrict, string> _repo;

        public GetSubDistrictByIdQueryHandler(IBaseRepositoryAsync<SubDistrict, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<SubDistrictsItem?> Handle(GetSubDistrictByIdQuery query, CancellationToken cancellationToken)
        {
            var items = await _repo.GetByIdAsync(query.Id, cancellationToken);

            var res = items != null ? new SubDistrictsItem
            {
                Id = items.Id,
                Name = items.Name,
            } : null;
            return res;
        }

    }
}
