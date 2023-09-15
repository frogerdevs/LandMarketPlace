namespace Subscription.Application.Features.UnitItems.Commands
{
    public partial class AddUnitItemCommand : ICommand<UnitItemResponse?>
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public int ValidDuration { get; set; }
        public int LiveDuration { get; set; }
        public required string BenefitType { get; set; }
        public string? BenefitSize { get; set; }
        public int QuantityUpload { get; set; }
        public int Priority { get; set; }
        public bool ShowInPackage { get; set; }
        public bool ShowInPageInDealPrice { get; set; }
        public bool ShowInPageInSpirationPrice { get; set; }
        public bool Active { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; } = 0;
        public int DiscountPercent { get; set; } = 0;
        public string? CreatedBy { get; set; }
    }
    public class AddUnitItemCommandHandler : ICommandHandler<AddUnitItemCommand, UnitItemResponse?>
    {
        private readonly IBaseRepositoryAsync<UnitItem, string> _repo;

        public AddUnitItemCommandHandler(IBaseRepositoryAsync<UnitItem, string> categoryRepo)
        {
            _repo = categoryRepo;
        }

        public async ValueTask<UnitItemResponse?> Handle(AddUnitItemCommand command, CancellationToken cancellationToken)
        {
            var entity = MapToEntity(command);
            var res = await _repo.AddAsync(entity, cancellationToken);
            return MapToItemResponse(res);
        }

        private static UnitItem MapToEntity(AddUnitItemCommand command)
        {
            return new UnitItem
            {
                Title = command.Title,
                Description = command.Description,
                ValidDuration = command.ValidDuration,
                LiveDuration = command.LiveDuration,
                Priority = command.Priority,
                ShowInPackage = command.ShowInPackage,
                ShowInPageInDealPrice = command.ShowInPageInDealPrice,
                ShowInPageInSpirationPrice = command.ShowInPageInSpirationPrice,
                BenefitType = command.BenefitType,
                BenefitSize = command.BenefitSize,
                QuantityUpload = command.QuantityUpload,
                Active = command.Active,
                Price = command.Price,
                DiscountPrice = command.DiscountPrice,
                DiscountPercent = command.DiscountPercent,
                CreatedBy = command.CreatedBy,
            };
        }

        private static UnitItemResponse MapToItemResponse(UnitItem res)
        {
            return new UnitItemResponse
            {
                Id = res.Id,
                Title = res.Title,
                Description = res.Description,
                ValidDuration = res.ValidDuration,
                LiveDuration = res.LiveDuration,
                BenefitType = res.BenefitType,
                BenefitSize = res.BenefitSize,
                QuantityUpload = res.QuantityUpload,
                Priority = res.Priority,
                ShowInPackage = res.ShowInPackage,
                ShowInPageInDealPrice = res.ShowInPageInDealPrice,
                ShowInPageInSpirationPrice = res.ShowInPageInSpirationPrice,
                Active = res.Active,
                Price = res.Price,
                DiscountPrice = res.DiscountPrice,
                DiscountPercent = res.DiscountPercent
            };
        }
    }

}
