namespace Subscription.Application.Features.UnitTypes.Queries
{
    public class GetUnitTypesQuery : IQuery<IEnumerable<UnitTypeItem>>
    {
    }
    public sealed class GetUnitTypesQueryHandler : IQueryHandler<GetUnitTypesQuery, IEnumerable<UnitTypeItem>>
    {
        private readonly IBaseRepositoryAsync<BenefitType, string> _repo;

        public GetUnitTypesQueryHandler(IBaseRepositoryAsync<BenefitType, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<IEnumerable<UnitTypeItem>> Handle(GetUnitTypesQuery query, CancellationToken cancellationToken)
        {
            var items = await _repo.NoTrackingEntities.Select(i => new UnitTypeItem
            {
                Id = i.Id,
                Name = i.Name!,
                Size = i.Size,
                Active = i.Active,
                Description = i.Description,

            }).ToListAsync(cancellationToken);

            return items;
        }
    }
}
