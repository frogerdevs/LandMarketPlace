namespace Subscription.Application.Features.UnitItems.Commands
{
    public partial class EditUnitItemCommand : ICommand<UnitItemResponse?>
    {
        public required string Id { get; set; }
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
        public BenefitType? UnitType { get; set; }
        public string? UpdateBy { get; set; }
    }
    public class EditUnitItemCommandHandler : ICommandHandler<EditUnitItemCommand, UnitItemResponse?>
    {
        private readonly IBaseRepositoryAsync<UnitItem, string> _repo;
        public EditUnitItemCommandHandler(IBaseRepositoryAsync<UnitItem, string> categoryRepo)
        {
            _repo = categoryRepo;
        }

        public async ValueTask<UnitItemResponse?> Handle(EditUnitItemCommand command, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetByIdAsync(command.Id, cancellationToken);
            if (entity == null)
            {
                return null!;
            }
            entity.Title = command.Title;
            entity.Description = command.Description;
            entity.ValidDuration = command.ValidDuration;
            entity.LiveDuration = command.LiveDuration;
            entity.BenefitType = command.BenefitType;
            entity.BenefitSize = command.BenefitSize;
            entity.QuantityUpload = command.QuantityUpload;
            entity.Priority = command.Priority;
            entity.ShowInPackage = command.ShowInPackage;
            entity.ShowInPageInDealPrice = command.ShowInPageInDealPrice;
            entity.ShowInPageInSpirationPrice = command.ShowInPageInSpirationPrice;

            entity.Active = command.Active;
            entity.Price = command.Price;
            entity.DiscountPrice = command.DiscountPrice;
            entity.DiscountPercent = command.DiscountPercent;
            entity.UpdatedBy = command.UpdateBy;

            var res = await _repo.UpdateAsync(entity, entity.Id, cancellationToken);
            return MaptoResponse(res);
        }

        private static UnitItemResponse MaptoResponse(UnitItem res)
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
