namespace Subscription.Application.Features.Vouchers.Queries
{
    public class GetVoucherByIdQuery : IQuery<VoucherItem?>
    {
        public string? Id { get; set; }
    }
    public sealed class GetVoucherItemQueryHandler : IQueryHandler<GetVoucherByIdQuery, VoucherItem?>
    {
        private readonly IBaseRepositoryAsync<Voucher, string> _repository;
        public GetVoucherItemQueryHandler(IBaseRepositoryAsync<Voucher, string> repository)
        {
            _repository = repository;
        }

        public async ValueTask<VoucherItem?> Handle(GetVoucherByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _repository.NoTrackingEntities
                            .Select(c => new VoucherItem
                            {
                                Id = c.Id,
                                Code = c.Code,
                                Name = c.Name,
                                Description = c.Description,
                                StarDate = c.StarDate,
                                EndDate = c.EndDate,
                                Duration = c.Duration,
                                Active = c.Active
                            }).FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            return item;
        }
    }
}
