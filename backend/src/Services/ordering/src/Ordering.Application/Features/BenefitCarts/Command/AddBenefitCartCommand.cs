namespace Ordering.Application.Features.BenefitCarts.Command
{
    public partial class AddBenefitCartCommand : ICommand<BenefitCartItem?>
    {
        public required string UserId { get; set; }
        public required string ItemType { get; set; }
        public string? PackageId { get; set; }
        public string? UnitItemId { get; set; }
        public string? SubscribeId { get; set; }
        public int Quantity { get; set; }
        public string? VoucherCode { get; set; }
    }
    public class AddBenefitCartCommandHandler : ICommandHandler<AddBenefitCartCommand, BenefitCartItem?>
    {
        private readonly IBaseRepositoryAsync<BenefitCart, string> _repo;

        public AddBenefitCartCommandHandler(IBaseRepositoryAsync<BenefitCart, string> categoryRepo)
        {
            _repo = categoryRepo;
        }

        public async ValueTask<BenefitCartItem?> Handle(AddBenefitCartCommand command, CancellationToken cancellationToken)
        {
            var entity = MapToEntity(command);
            var res = await _repo.AddAsync(entity, cancellationToken);
            return MapToItemResponse(res);
        }

        private static BenefitCart MapToEntity(AddBenefitCartCommand command)
        {
            return new BenefitCart
            {
                UserId = command.UserId,
                ItemType = command.ItemType,
                PackageId = command.PackageId,
                UnitItemId = command.UnitItemId,
                SubscribeId = command.SubscribeId,
                Quantity = command.Quantity,
                VoucherCode = command.VoucherCode
            };
        }

        private static BenefitCartItem MapToItemResponse(BenefitCart res)
        {
            return new BenefitCartItem
            {
                Id = res.Id,
                UserId = res.UserId,
                ItemType = res.ItemType,
                PackageId = res.PackageId,
                UnitItemId = res.UnitItemId,
                SubscribeId = res.SubscribeId,
                Quantity = res.Quantity,
                VoucherCode = res.VoucherCode
            };
        }
    }

}
