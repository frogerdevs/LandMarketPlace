namespace Subscription.Application.Features.UnitTypes.Queries
{
    public class GetUnitTypeByIdQuery : IQuery<UnitTypeItem?>
    {
        public required string Id { get; set; }
    }
    public sealed class GetUnitTypeByIdQueryHandler : IQueryHandler<GetUnitTypeByIdQuery, UnitTypeItem?>
    {
        private readonly IBaseRepositoryAsync<BenefitType, string> _repo;

        public GetUnitTypeByIdQueryHandler(IBaseRepositoryAsync<BenefitType, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<UnitTypeItem?> Handle(GetUnitTypeByIdQuery query, CancellationToken cancellationToken)
        {
            var items = await _repo.GetByIdAsync(query.Id, cancellationToken);

            return MapToResponse(items);
        }

        private static UnitTypeItem? MapToResponse(BenefitType? items)
        {
            return items != null ? new UnitTypeItem
            {
                Id = items.Id,
                Name = items.Name,
                Description = items.Description,
                Size = items.Size,
                Active = items.Active
            } : null;
        }
    }
}
