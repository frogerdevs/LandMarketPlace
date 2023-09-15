namespace Subscription.Application.Features.PackageSubscribe.Commands
{
    public class AddPackageCommand : ICommand<PackageItem?>
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public int DiscountPercent { get; set; }
        public int Priority { get; set; }
        public string? ImageUrl { get; set; }
        public bool Active { get; set; }
        public string? CreatedBy { get; set; }
        public List<PackageDetailRequest>? PackageDetails { get; set; }
    }
    public class PackageDetailRequest
    {
        public required string PackageId { get; set; }
        public required string UnitItemId { get; set; }
        public string? ImageUrl { get; set; }
        public int Quantity { get; set; }
    }

    public sealed class AddPackageCommandHandler : ICommandHandler<AddPackageCommand, PackageItem?>
    {
        private readonly IBaseRepositoryAsync<Package, string> _repo;

        public AddPackageCommandHandler(IBaseRepositoryAsync<Package, string> categoryRepo)
        {
            _repo = categoryRepo;
        }

        public async ValueTask<PackageItem?> Handle(AddPackageCommand command, CancellationToken cancellationToken)
        {
            var entity = MapToEntity(command);
            var res = await _repo.AddAsync(entity, cancellationToken);
            return MapToItemResponse(res);
        }

        private static Package MapToEntity(AddPackageCommand command)
        {
            return new Package
            {
                Title = command.Title,
                Description = command.Description,
                Duration = command.Duration,
                Price = command.Price,
                DiscountPrice = command.DiscountPrice,
                DiscountPercent = command.DiscountPercent,
                Priority = command.Priority,
                ImageUrl = command.ImageUrl,
                Active = command.Active,
                CreatedBy = command.CreatedBy,
            };
        }

        private static PackageItem MapToItemResponse(Package res)
        {
            return new PackageItem
            {
                Id = res.Id,
                Title = res.Title,
                Description = res.Description,
                Duration = res.Duration,
                Price = res.Price,
                DiscountPrice = res.DiscountPrice,
                DiscountPercent = res.DiscountPercent,
                Priority = res.Priority,
                ImageUrl = res.ImageUrl,
                Active = res.Active
            };
        }
    }

}
