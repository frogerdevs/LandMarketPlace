namespace Subscription.Application.Features.UnitItems.Queries
{
    public class GetUnitItemByIdQuery : IQuery<UnitItemResponse?>
    {
        public required string Id { get; set; }
    }
    public sealed class UnitItemByIdQueryHandler : IQueryHandler<GetUnitItemByIdQuery, UnitItemResponse?>
    {
        private readonly IBaseRepositoryAsync<UnitItem, string> _repo;

        public UnitItemByIdQueryHandler(IBaseRepositoryAsync<UnitItem, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<UnitItemResponse?> Handle(GetUnitItemByIdQuery query, CancellationToken cancellationToken)
        {
            var items = await _repo.GetByIdAsync(query.Id, cancellationToken);

            return MapToResponse(items);
        }

        private static UnitItemResponse? MapToResponse(UnitItem? items)
        {
            return items != null ? new UnitItemResponse
            {
                Id = items.Id,
                Title = items.Title,
                Description = items.Description,
                ValidDuration = items.ValidDuration,
                LiveDuration = items.LiveDuration,
                BenefitType = items.BenefitType,
                BenefitSize = items.BenefitSize,
                QuantityUpload = items.QuantityUpload,
                Priority = items.Priority,
                ShowInPackage = items.ShowInPackage,
                ShowInPageInDealPrice = items.ShowInPageInDealPrice,
                ShowInPageInSpirationPrice = items.ShowInPageInSpirationPrice,
                Active = items.Active,
                Price = items.Price,
                DiscountPrice = items.DiscountPrice,
                DiscountPercent = items.DiscountPercent
            } : null;
        }
    }

}
