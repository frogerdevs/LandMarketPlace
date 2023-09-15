namespace Subscription.Application.Features.Subscribes.Queries
{
    public class GetSubscribeByIdQuery : IQuery<SubscribeItem?>
    {
        public string? Id { get; set; }
    }
    public sealed class GetVoucherItemQueryHandler : IQueryHandler<GetSubscribeByIdQuery, SubscribeItem?>
    {
        private readonly IBaseRepositoryAsync<Subscribe, string> _repository;
        public GetVoucherItemQueryHandler(IBaseRepositoryAsync<Subscribe, string> repository)
        {
            _repository = repository;
        }

        public async ValueTask<SubscribeItem?> Handle(GetSubscribeByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _repository.NoTrackingEntities
                            .Select(c => new SubscribeItem
                            {
                                Id = c.Id,
                                Name = c.Name,
                                Description = c.Description,
                                Price = c.Price,
                                DurationDays = c.DurationDays,
                                UpgradableFrom = c.UpgradableFrom,
                                DiscountPrice = c.DiscountPrice,
                                DiscountPercent = c.DiscountPercent,
                                Active = c.Active
                            }).FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            return item;
        }
    }
}
