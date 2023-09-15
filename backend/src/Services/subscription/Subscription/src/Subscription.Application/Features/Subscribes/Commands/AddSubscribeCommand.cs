namespace Subscription.Application.Features.Subscribes.Commands
{
    public partial class AddSubscribeCommand : ICommand<SubscribeItem?>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public string? UpgradableFrom { get; set; }
        public decimal DiscountPrice { get; set; } = 0;
        public int DiscountPercent { get; set; } = 0;
        public bool Active { get; set; }
        public string? CreatedBy { get; set; }
    }
    public class AddSubscribeCommandHandler : ICommandHandler<AddSubscribeCommand, SubscribeItem?>
    {
        private readonly IBaseRepositoryAsync<Subscribe, string> _repo;

        public AddSubscribeCommandHandler(IBaseRepositoryAsync<Subscribe, string> categoryRepo)
        {
            _repo = categoryRepo;
        }

        public async ValueTask<SubscribeItem?> Handle(AddSubscribeCommand command, CancellationToken cancellationToken)
        {
            var entity = MapToEntity(command);
            var res = await _repo.AddAsync(entity, cancellationToken);
            return MapToItemResponse(res);
        }

        private static Subscribe MapToEntity(AddSubscribeCommand command)
        {
            return new Subscribe
            {
                Name = command.Name,
                Description = command.Description,
                Price = command.Price,
                DurationDays = command.DurationDays,
                UpgradableFrom = command.UpgradableFrom,
                DiscountPrice = command.DiscountPrice,
                DiscountPercent = command.DiscountPercent,
                Active = command.Active,
                CreatedBy = command.CreatedBy
            };
        }

        private static SubscribeItem MapToItemResponse(Subscribe res)
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
