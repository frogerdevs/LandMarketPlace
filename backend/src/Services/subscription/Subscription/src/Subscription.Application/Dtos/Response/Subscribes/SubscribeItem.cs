namespace Subscription.Application.Dtos.Response.Subscribes
{
    public class SubscribeItem
    {
        public required string Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public string? UpgradableFrom { get; set; }
        public decimal DiscountPrice { get; set; } = 0;
        public int DiscountPercent { get; set; } = 0;
        public bool Active { get; set; }
    }
}
