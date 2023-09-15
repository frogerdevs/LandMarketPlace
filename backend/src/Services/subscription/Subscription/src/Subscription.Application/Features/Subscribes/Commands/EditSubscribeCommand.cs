namespace Subscription.Application.Features.Subscribes.Commands
{
    public partial class EditSubscribeCommand : ICommand<SubscribeItem?>
    {
        public required string Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public string? UpgradableFrom { get; set; }
        public decimal DiscountPrice { get; set; } = 0;
        public int DiscountPercent { get; set; } = 0;
        public bool Active { get; set; }
        public string? UpdateBy { get; set; }
    }
    public class EditSubscribeCommandHandler : ICommandHandler<EditSubscribeCommand, SubscribeItem?>
    {
        private readonly IBaseRepositoryAsync<Subscribe, string> _repo;
        public EditSubscribeCommandHandler(IBaseRepositoryAsync<Subscribe, string> categoryRepo)
        {
            _repo = categoryRepo;
        }

        public async ValueTask<SubscribeItem?> Handle(EditSubscribeCommand command, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetByIdAsync(command.Id, cancellationToken);
            if (entity == null)
            {
                return null!;
            }
            entity.Name = command.Name;
            entity.Description = command.Description;
            entity.Price = command.Price;
            entity.DurationDays = command.DurationDays;
            entity.UpgradableFrom = command.UpgradableFrom;
            entity.DiscountPrice = command.DiscountPrice;
            entity.DiscountPercent = command.DiscountPercent;
            entity.Active = command.Active;
            entity.UpdatedBy = command.UpdateBy;

            var res = await _repo.UpdateAsync(entity, entity.Id, cancellationToken);
            return MaptoResponse(res);
        }

        private static SubscribeItem MaptoResponse(Subscribe res)
        {
            return new SubscribeItem
            {
                Id = res.Id,
                Name = res.Name,
                Description = res.Description,
                Price = res.Price,
                DurationDays = res.DurationDays,
                UpgradableFrom = res.UpgradableFrom,
                DiscountPrice = res.DiscountPrice,
                DiscountPercent = res.DiscountPercent,
                Active = res.Active
            };
        }
    }

}
