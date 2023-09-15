namespace Subscription.Application.Dtos.Response.UnitItems
{
    public class UnitItemResponse
    {
        public required string Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public int ValidDuration { get; set; }
        public int LiveDuration { get; set; }
        public required string BenefitType { get; set; }
        public string? BenefitSize { get; set; }
        public int QuantityUpload { get; set; }
        public int Priority { get; set; }
        public bool ShowInPackage { get; set; }
        public bool ShowInPageInDealPrice { get; set; }
        public bool ShowInPageInSpirationPrice { get; set; }
        public bool Active { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; } = 0;
        public int DiscountPercent { get; set; } = 0;
    }
}
