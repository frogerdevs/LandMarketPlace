namespace Subscription.Application.Features.UnitItems.Queries
{
    public class GetUnitItemsQuery : IQuery<IEnumerable<UnitItemResponse>>
    {
    }
    public sealed class GetUnitItemsQueryHandler : IQueryHandler<GetUnitItemsQuery, IEnumerable<UnitItemResponse>>
    {
        private readonly IBaseRepositoryAsync<UnitItem, string> _repo;

        public GetUnitItemsQueryHandler(IBaseRepositoryAsync<UnitItem, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<IEnumerable<UnitItemResponse>> Handle(GetUnitItemsQuery query, CancellationToken cancellationToken)
        {
            var items = await _repo.NoTrackingEntities.Select(i => new UnitItemResponse
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                ValidDuration = i.ValidDuration,
                LiveDuration = i.LiveDuration,
                BenefitType = i.BenefitType,
                BenefitSize = i.BenefitSize,
                QuantityUpload = i.QuantityUpload,
                Priority = i.Priority,
                ShowInPackage = i.ShowInPackage,
                ShowInPageInDealPrice = i.ShowInPageInDealPrice,
                ShowInPageInSpirationPrice = i.ShowInPageInSpirationPrice,
                Active = i.Active,
                Price = i.Price,
                DiscountPrice = i.DiscountPrice,
                DiscountPercent = i.DiscountPercent,
                //BenefitType = i.BenefitType,
            }).ToListAsync(cancellationToken);

            return items;
        }
    }

}
