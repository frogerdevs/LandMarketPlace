namespace Subscription.Application.Features.Vouchers.Queries
{
    public class GetVouchersQuery : IQuery<IEnumerable<VoucherItem>>
    {
    }
    public sealed class GetVouchersQueryHandler : IQueryHandler<GetVouchersQuery, IEnumerable<VoucherItem>>
    {
        private readonly IBaseRepositoryAsync<Voucher, string> _repo;

        public GetVouchersQueryHandler(IBaseRepositoryAsync<Voucher, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<IEnumerable<VoucherItem>> Handle(GetVouchersQuery query, CancellationToken cancellationToken)
        {
            var items = await _repo.NoTrackingEntities.Select(i => new VoucherItem
            {
                Id = i.Id,
                Name = i.Name!,
                Code = i.Code,
                Duration = i.Duration,
                StarDate = i.StarDate,
                EndDate = i.EndDate,
                Active = i.Active,
                Description = i.Description,
            }).ToListAsync(cancellationToken);

            return items;
        }
    }
}
