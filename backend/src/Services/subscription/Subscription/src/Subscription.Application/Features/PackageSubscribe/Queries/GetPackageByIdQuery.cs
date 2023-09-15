namespace Subscription.Application.Features.PackageSubscribe.Queries
{
    public class GetPackageByIdQuery : IQuery<PackageItem?>
    {
        public string? Id { get; set; }
    }
    public sealed class GetVoucherItemQueryHandler : IQueryHandler<GetPackageByIdQuery, PackageItem?>
    {
        private readonly IBaseRepositoryAsync<Package, string> _repository;
        public GetVoucherItemQueryHandler(IBaseRepositoryAsync<Package, string> repository)
        {
            _repository = repository;
        }

        public async ValueTask<PackageItem?> Handle(GetPackageByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _repository.NoTrackingEntities
                            .Select(c => new PackageItem
                            {
                                Id = c.Id,
                                Title = c.Title,
                                Description = c.Description,
                                Duration = c.Duration,
                                Price = c.Price,
                                DiscountPrice = c.DiscountPrice,
                                DiscountPercent = c.DiscountPercent,
                                Priority = c.Priority,
                                ImageUrl = c.ImageUrl,
                                Active = c.Active
                            }).FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            return item;
        }
    }
}
