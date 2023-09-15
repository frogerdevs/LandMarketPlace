namespace Subscription.Application.Features.PackageSubscribe.Queries
{
    public class GetPackagesQuery : IQuery<IEnumerable<PackageItem>>
    {
    }
    public sealed class GetPackagesQueryHandler : IQueryHandler<GetPackagesQuery, IEnumerable<PackageItem>>
    {
        private readonly IBaseRepositoryAsync<Package, string> _repo;

        public GetPackagesQueryHandler(IBaseRepositoryAsync<Package, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<IEnumerable<PackageItem>> Handle(GetPackagesQuery query, CancellationToken cancellationToken)
        {
            var items = await _repo.NoTrackingEntities.Select(i => new PackageItem
            {
                Id = i.Id,
                Title = i.Title!,
                DiscountPercent = i.DiscountPercent,
                DiscountPrice = i.DiscountPrice,
                Duration = i.Duration,
                Price = i.Price,
                Active = i.Active,
                Priority = i.Priority,
                ImageUrl = i.ImageUrl,
                Description = i.Description,
            }).ToListAsync(cancellationToken);

            return items;
        }
    }
}
