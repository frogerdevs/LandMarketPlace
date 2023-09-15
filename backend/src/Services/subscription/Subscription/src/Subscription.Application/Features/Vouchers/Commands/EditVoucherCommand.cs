namespace Subscription.Application.Features.Vouchers.Commands
{
    public partial class EditVoucherCommand : ICommand<VoucherItem?>
    {
        public required string Id { get; set; }
        public required string Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StarDate { get; set; } = default;
        public DateTime? EndDate { get; set; } = default;
        public int Duration { get; set; }
        public bool Active { get; set; }
        public string? UpdateBy { get; set; }
    }
    public class EditVoucherCommandHandler : ICommandHandler<EditVoucherCommand, VoucherItem?>
    {
        private readonly IBaseRepositoryAsync<Voucher, string> _repo;
        public EditVoucherCommandHandler(IBaseRepositoryAsync<Voucher, string> categoryRepo)
        {
            _repo = categoryRepo;
        }

        public async ValueTask<VoucherItem?> Handle(EditVoucherCommand command, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetByIdAsync(command.Id, cancellationToken);
            if (entity == null)
            {
                return null!;
            }
            entity.Name = command.Name;
            entity.Description = command.Description;
            entity.Code = command.Code;
            entity.StarDate = command.StarDate;
            entity.EndDate = command.EndDate;
            entity.Duration = command.Duration;
            entity.Active = command.Active;
            entity.UpdatedBy = command.UpdateBy;

            var res = await _repo.UpdateAsync(entity, entity.Id, cancellationToken);
            return MaptoResponse(res);
        }

        private static VoucherItem MaptoResponse(Voucher res)
        {
            return new VoucherItem
            {
                Id = res.Id,
                Code = res.Code,
                Name = res.Name,
                Description = res.Description,
                StarDate = res.StarDate,
                EndDate = res.EndDate,
                Duration = res.Duration,
                Active = res.Active
            };
        }
    }
}
