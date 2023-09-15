namespace Subscription.Domain.Entities.Vouchers
{
    public class UserVoucher : BaseEntity<string>
    {
        public string? UserId { get; set; }
        public string? VoucherId { get; set; }
        public bool Used { get; set; }
        public Voucher? Voucher { get; set; }
        public UserVoucher()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
