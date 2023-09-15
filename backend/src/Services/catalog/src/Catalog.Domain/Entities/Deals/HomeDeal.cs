namespace Catalog.Domain.Entities.Deals
{
    public class HomeDeal : BaseAuditableEntity<string>
    {
        public required string ProductId { get; set; }
        public string? ImgUrl { get; set; }
        public bool Active { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Channel { get; set; } = "unknown";
        public virtual Product? Product { get; set; }
        public HomeDeal()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
