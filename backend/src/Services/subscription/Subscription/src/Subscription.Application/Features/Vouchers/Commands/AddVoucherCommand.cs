namespace Subscription.Application.Features.Vouchers.Commands
{
    public partial class AddVoucherCommand : ICommand<VoucherItem?>
    {
        public required string Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StarDate { get; set; } = default;
        public DateTime? EndDate { get; set; } = default;
        public int Duration { get; set; }
        public bool Active { get; set; }
        public string? CreatedBy { get; set; }
    }
    public class AddVoucherCommandHandler : ICommandHandler<AddVoucherCommand, VoucherItem?>
    {
        private readonly IBaseRepositoryAsync<Voucher, string> _repo;

        public AddVoucherCommandHandler(IBaseRepositoryAsync<Voucher, string> categoryRepo)
        {
            _repo = categoryRepo;
        }

        public async ValueTask<VoucherItem?> Handle(AddVoucherCommand command, CancellationToken cancellationToken)
        {
            var entity = MapToEntity(command);
            var res = await _repo.AddAsync(entity, cancellationToken);
            return MapToItemResponse(res);
        }

        private static Voucher MapToEntity(AddVoucherCommand command)
        {
            return new Voucher
            {
                Code = command.Code,
                Name = command.Name,
                Description = command.Description,
                StarDate = command.StarDate,
                EndDate = command.EndDate,
                Duration = command.Duration,
                Active = command.Active,
                CreatedBy = command.CreatedBy,
            };
        }

        private static VoucherItem MapToItemResponse(Voucher res)
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
