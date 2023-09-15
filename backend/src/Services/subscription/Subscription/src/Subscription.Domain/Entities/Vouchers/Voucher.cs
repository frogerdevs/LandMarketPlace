namespace Subscription.Domain.Entities.Vouchers
{
    public class Voucher : BaseAuditableEntity<string>
    {
        public required string Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StarDate { get; set; } = default;
        public DateTime? EndDate { get; set; } = default;
        public int Duration { get; set; }
        public bool Active { get; set; }
        public ICollection<UserVoucher>? UserVouchers { get; set; }
        public Voucher()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
