namespace Subscription.Domain.Entities.Subscriptions
{
    public class Subscribe : BaseAuditableEntity<string>
    {
        public Subscribe()
        {
            Id = Guid.NewGuid().ToString();
        }
        public Subscribe(string id)
        {
            Id = id;
        }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public string? UpgradableFrom { get; set; }
        public decimal DiscountPrice { get; set; } = 0;
        public int DiscountPercent { get; set; } = 0;
        public bool Active { get; set; }
        public ICollection<SubscribeDetail>? SubscribeDetails { get; set; }
        //public ICollection<BenefitCart>? BenefitCarts { get; set; }
    }
}
