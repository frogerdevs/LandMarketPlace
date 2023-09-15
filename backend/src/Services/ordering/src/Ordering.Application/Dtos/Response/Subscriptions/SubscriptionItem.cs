namespace Ordering.Application.Dtos.Response.Subscriptions
{
    public class SubscriptionItem
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public string? UpgradableFrom { get; set; }
        public bool Active { get; set; }
        public ICollection<SubscriptionItemDetail>? SubscriptionDetails { get; set; }

    }
}
