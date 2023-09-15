namespace Ordering.Application.Features.BenefitCarts.Queries
{
    public class GetBenefitCartsQuery : IQuery<IEnumerable<BenefitCartItem>>
    {
    }
    public sealed class GetBenefitCartsQueryHandler : IQueryHandler<GetBenefitCartsQuery, IEnumerable<BenefitCartItem>>
    {
        private readonly IBaseRepositoryAsync<BenefitCart, string> _repo;

        public GetBenefitCartsQueryHandler(IBaseRepositoryAsync<BenefitCart, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<IEnumerable<BenefitCartItem>> Handle(GetBenefitCartsQuery query, CancellationToken cancellationToken)
        {
            var items = await _repo.NoTrackingEntities.Select(i => new BenefitCartItem
            {
                Id = i.Id,
                UserId = i.UserId,
                ItemType = i.ItemType,
                PackageId = i.PackageId,
                UnitItemId = i.UnitItemId,
                SubscribeId = i.SubscribeId,
                Quantity = i.Quantity,
                VoucherCode = i.VoucherCode
            }).ToListAsync(cancellationToken);

            return items;
        }
    }
}
