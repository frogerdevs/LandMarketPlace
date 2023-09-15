namespace Subscription.Application.Features.PackageSubscribe.Commands
{
    public partial class EditPackageCommand : ICommand<PackageItem?>
    {
        public required string Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public int DiscountPercent { get; set; }
        public int Priority { get; set; }
        public string? ImageUrl { get; set; }
        public bool Active { get; set; }
        public string? UpdateBy { get; set; }
    }
    public class EditPackageCommandHandler : ICommandHandler<EditPackageCommand, PackageItem?>
    {
        private readonly IBaseRepositoryAsync<Package, string> _repo;
        public EditPackageCommandHandler(IBaseRepositoryAsync<Package, string> categoryRepo)
        {
            _repo = categoryRepo;
        }

        public async ValueTask<PackageItem?> Handle(EditPackageCommand command, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetByIdAsync(command.Id, cancellationToken);
            if (entity == null)
            {
                return null!;
            }
            entity.Title = command.Title;
            entity.Description = command.Description;
            entity.Duration = command.Duration;
            entity.Price = command.Price;
            entity.DiscountPrice = command.DiscountPrice;
            entity.DiscountPercent = command.DiscountPercent;
            entity.Priority = command.Priority;
            entity.ImageUrl = command.ImageUrl;
            entity.Active = command.Active;
            entity.UpdatedBy = command.UpdateBy;

            var res = await _repo.UpdateAsync(entity, entity.Id, cancellationToken);
            return MaptoResponse(res);
        }

        private static PackageItem MaptoResponse(Package res)
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
