namespace Ordering.Domain.Entities.Benefit
{
    public class BenefitCart : BaseEntity<string>
    {
        public required string UserId { get; set; }
        public required string ItemType { get; set; }
        public string? PackageId { get; set; }
        public string? UnitItemId { get; set; }
        public string? SubscribeId { get; set; }
        public int Quantity { get; set; }
        public string? VoucherCode { get; set; }
        //public Package? Package { get; set; }
        //public UnitItem? UnitItem { get; set; }
        //public Subscribe? Subscribe { get; set; }
        public BenefitCart()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public enum ItemTypes
    {
        UnitItem = 1,
        Package = 2,
        Subscription = 3
    }
}
