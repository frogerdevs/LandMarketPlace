namespace Ordering.Domain.Entities.Benefit
{
    public class OrderSubscribe : BaseAuditableEntity<string>
    {
        public required string BenefitOrderId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public string? UpgradableFrom { get; set; }
        public decimal DiscountPrice { get; set; } = 0;
        public int DiscountPercent { get; set; } = 0;
        public BenefitOrder? BenefitOrder { get; set; }
        public DateTime? ActivateDate { get; set; }//this will fill after status paid
        public DateTime? ExpiredDate { get; set; }//this will fill after status paid
        public DateTime? UsedDate { get; set; }//this will fill after User used this subscription
        public bool IsUsed
        {
            get => UsedDate.HasValue;
        }
        public OrderSubscribe()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
