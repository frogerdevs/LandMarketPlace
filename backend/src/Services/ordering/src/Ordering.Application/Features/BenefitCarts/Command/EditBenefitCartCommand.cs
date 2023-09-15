namespace Ordering.Application.Features.BenefitCarts.Command
{
    public partial class EditBenefitCartCommand : ICommand<BenefitCartItem?>
    {
        public required string Id { get; set; }
        public required string UserId { get; set; }
        public required string ItemType { get; set; }
        public string? PackageId { get; set; }
        public string? UnitItemId { get; set; }
        public string? SubscribeId { get; set; }
        public int Quantity { get; set; }
        public string? VoucherCode { get; set; }
    }
    public class EditBenefitCartCommandHandler : ICommandHandler<EditBenefitCartCommand, BenefitCartItem?>
    {
        private readonly IBaseRepositoryAsync<BenefitCart, string> _repo;
        public EditBenefitCartCommandHandler(IBaseRepositoryAsync<BenefitCart, string> categoryRepo)
        {
            _repo = categoryRepo;
        }

        public async ValueTask<BenefitCartItem?> Handle(EditBenefitCartCommand command, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetByIdAsync(command.Id, cancellationToken);
            if (entity == null)
            {
                return null!;
            }
            entity.UserId = command.UserId;
            entity.ItemType = command.ItemType;
            entity.PackageId = command.PackageId;
            entity.UnitItemId = command.UnitItemId;
            entity.SubscribeId = command.SubscribeId;
            entity.Quantity = command.Quantity;
            entity.VoucherCode = command.VoucherCode;

            var res = await _repo.UpdateAsync(entity, entity.Id, cancellationToken);
            return MaptoResponse(res);
        }

        private static BenefitCartItem MaptoResponse(BenefitCart res)
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
