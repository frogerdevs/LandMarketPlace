namespace Ordering.Application.Dtos.Response.BenefitCarts
{
    public class BenefitCartItem
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
}
