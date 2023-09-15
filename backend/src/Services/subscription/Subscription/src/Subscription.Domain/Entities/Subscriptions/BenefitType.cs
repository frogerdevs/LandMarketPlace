namespace Subscription.Domain.Entities.Subscriptions
{
    public class BenefitType : BaseAuditableEntity<string>
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Size { get; set; }
        public bool ShowInSubscribe { get; set; }
        public bool ShowInUnitItem { get; set; }
        public bool Active { get; set; }
        public ICollection<UnitItem>? UnitItems { get; set; }
        public BenefitType()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
