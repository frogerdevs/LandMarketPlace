namespace Ordering.Domain.Entities.Benefit
{
    public class OrderPackage : BaseAuditableEntity<string>
    {
        public required string BenefitOrderId { get; set; }
        public required string PackageId { get; set; }
        public required string PackageName { get; set; }
        public string? PackageDescription { get; set; }
        public int PackageDuration { get; set; }
        public decimal PackagePrice { get; set; }
        public decimal PackageDiscountPrice { get; set; } = 0;
        public int PackageDiscountPercent { get; set; } = 0;
        public decimal PackageTotalPrice { get; set; } = 0;
        public DateTime ActivateDate { get; set; }//this will fill after status paid
        public DateTime ExpiredDate { get; set; }//this will fill after status paid
        public BenefitOrder? BenefitOrder { get; set; }
        public OrderPackage()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
