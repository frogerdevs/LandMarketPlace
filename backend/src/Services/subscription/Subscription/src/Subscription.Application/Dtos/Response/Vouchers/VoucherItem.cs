namespace Subscription.Application.Dtos.Response.Vouchers
{
    public class VoucherItem
    {
        public required string Id { get; set; }
        public required string Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StarDate { get; set; } = default;
        public DateTime? EndDate { get; set; } = default;
        public int Duration { get; set; }
        public bool Active { get; set; }
    }
}
