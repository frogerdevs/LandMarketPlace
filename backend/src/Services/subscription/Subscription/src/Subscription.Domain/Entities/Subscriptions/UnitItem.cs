namespace Subscription.Domain.Entities.Subscriptions
{
    public class UnitItem : BaseAuditableEntity<string>
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public int LiveDuration { get; set; }
        public int ValidDuration { get; set; }
        public int QuantityUpload { get; set; }
        public bool Active { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; } = 0;
        public int DiscountPercent { get; set; } = 0;
        public bool ShowInPackage { get; set; }
        public bool ShowInPageInDealPrice { get; set; }
        public bool ShowInPageInSpirationPrice { get; set; }
        public int Priority { get; set; }
        public required string BenefitType { get; set; }
        public string? BenefitSize { get; set; }
        //public BenefitType? UnitType { get; set; }
        public ICollection<PackageDetail>? PackageDetails { get; set; }
        //public ICollection<BenefitCart>? BenefitCarts { get; set; }
        public UnitItem()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

}
