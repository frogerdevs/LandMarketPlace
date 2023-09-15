namespace Subscription.Application.Features.Subscribes.Queries
{
    public class GetSubscribesQuery : IQuery<IEnumerable<SubscribeItem>>
    {
    }
    public sealed class GetSubscribesQueryHandler : IQueryHandler<GetSubscribesQuery, IEnumerable<SubscribeItem>>
    {
        private readonly IBaseRepositoryAsync<Subscribe, string> _repo;

        public GetSubscribesQueryHandler(IBaseRepositoryAsync<Subscribe, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<IEnumerable<SubscribeItem>> Handle(GetSubscribesQuery query, CancellationToken cancellationToken)
        {
            var items = await _repo.NoTrackingEntities.Select(i => new SubscribeItem
            {
                Id = i.Id,
                Name = i.Name!,
                DiscountPercent = i.DiscountPercent,
                DiscountPrice = i.DiscountPrice,
                DurationDays = i.DurationDays,
                Price = i.Price,
                UpgradableFrom = i.UpgradableFrom,
                Active = i.Active,
                Description = i.Description,
            }).ToListAsync(cancellationToken);

            return items;
        }
    }
}
