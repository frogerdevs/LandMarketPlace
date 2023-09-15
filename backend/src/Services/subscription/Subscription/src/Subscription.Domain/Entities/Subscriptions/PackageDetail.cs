namespace Subscription.Domain.Entities.Subscriptions
{
    public class PackageDetail : BaseEntity
    {
        public required string PackageId { get; set; }
        public required string UnitItemId { get; set; }
        public string? ImageUrl { get; set; }
        public int Quantity { get; set; }
        public Package? Package { get; set; }
        public UnitItem? UnitItem { get; set; }
    }
}
