namespace Ordering.Domain.Entities.Benefit
{
    public class OrderUnitItem : BaseAuditableEntity<string>
    {
        public required string BenefitOrderId { get; set; }
        public string? OrderPackageId { get; set; }
        public required string UnitItemId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; }
        public required string UnitTypeName { get; set; }
        public int QuantityUpload { get; set; }
        public bool Active { get; set; }
        public decimal Price { get; set; } = 0;
        public decimal DiscountPrice { get; set; } = 0;
        public int DiscountPercent { get; set; } = 0;
        public DateTime? ActivateDate { get; set; }//this will fill after status paid
        public DateTime? ExpiredDate { get; set; }//this will fill after status paid
        public DateTime? UsedDate { get; set; }//this will fill after User used this subscription
        public bool IsUsed
        {
            get => UsedDate.HasValue;
        }
        public OrderPackage? OrderPackage { get; set; }
        public BenefitOrder? BenefitOrder { get; set; }

        public OrderUnitItem()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
