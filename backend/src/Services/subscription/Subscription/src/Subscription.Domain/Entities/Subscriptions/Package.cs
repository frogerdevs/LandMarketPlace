namespace Subscription.Domain.Entities.Subscriptions
{
    public class Package : BaseAuditableEntity<string>
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public int DiscountPercent { get; set; }
        public bool Active { get; set; }
        public int Priority { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<PackageDetail>? PackageDetails { get; set; }
        //public ICollection<BenefitCart>? BenefitCarts { get; set; }
        public Package()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
