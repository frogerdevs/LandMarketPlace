namespace Ordering.Domain.Entities.Benefit
{
    public class BenefitOrder : BaseAuditableEntity<string>
    {
        public required string UserId { get; set; }
        public string? OrderNumber { get; set; }
        public required string ItemType { get; set; }
        public string? PackageId { get; set; }
        public string? UnitItemId { get; set; }
        public string? SubscriptionId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public int DiscountPercent { get; set; }
        public string? VoucherCode { get; set; }
        public decimal TotalAfterDiscount { get; set; }
        public int Tax { get; set; }
        public decimal TotalAfterTax { get; set; }
        public decimal GrandTotalPrice { get; set; }
        public string? OrderStatus { get; set; }
        public DateTime? PaidDate { get; set; }//this will fill after status paid
        public ICollection<OrderPackage>? OrderPackages { get; set; }
        public ICollection<OrderUnitItem>? OrderUnitItems { get; set; }
        public ICollection<OrderSubscribe>? OrderSubscriptions { get; set; }

        public BenefitOrder()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public enum OrderStatus
    {
        Deleted = 0,
        Submitted = 1,
        WaitingPayment = 2,
        WaitingConfirmation = 3,
        Paid = 4,
        Failed = 5,
        Cancelled = 6,
    }
}
