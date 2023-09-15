namespace Ordering.Application.Features.BenefitCarts.Queries
{
    public class GetBenefitCartByIdQuery : IQuery<BenefitCartItem?>
    {
        public string? Id { get; set; }
    }
    public sealed class GetVoucherItemQueryHandler : IQueryHandler<GetBenefitCartByIdQuery, BenefitCartItem?>
    {
        private readonly IBaseRepositoryAsync<BenefitCart, string> _repository;
        public GetVoucherItemQueryHandler(IBaseRepositoryAsync<BenefitCart, string> repository)
        {
            _repository = repository;
        }

        public async ValueTask<BenefitCartItem?> Handle(GetBenefitCartByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _repository.NoTrackingEntities
                            .Select(c => new BenefitCartItem
                            {
                                Id = c.Id,
                                UserId = c.UserId,
                                ItemType = c.ItemType,
                                PackageId = c.PackageId,
                                UnitItemId = c.UnitItemId,
                                SubscribeId = c.SubscribeId,
                                Quantity = c.Quantity,
                                VoucherCode = c.VoucherCode
                            }).FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            return item;
        }
    }
}
