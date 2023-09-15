namespace IdentityServer.Features.Districts.Queries
{
    public class GetDistrictByIdQuery : IQuery<DistrictItem?>
    {
        public required string Id { get; set; }
    }
    public sealed class GetDistrictByIdQueryHandler : IQueryHandler<GetDistrictByIdQuery, DistrictItem?>
    {
        private readonly IBaseRepositoryAsync<District, string> _repo;

        public GetDistrictByIdQueryHandler(IBaseRepositoryAsync<District, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<DistrictItem?> Handle(GetDistrictByIdQuery query, CancellationToken cancellationToken)
        {
            var items = await _repo.GetByIdAsync(query.Id, cancellationToken);

            var res = items != null ? new DistrictItem
            {
                Id = items.Id,
                Name = items.Name,
            } : null;
            return res;
        }

    }
}
