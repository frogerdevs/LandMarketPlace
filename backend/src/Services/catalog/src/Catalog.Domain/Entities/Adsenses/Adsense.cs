namespace Catalog.Domain.Entities.Adsenses
{
    public class Adsense : BaseAuditableEntity<string>
    {
        public required string ProductId { get; set; }
        public required string UserId { get; set; }
        public string? ImageUrl { get; set; }
        public required string Title { get; set; }
        public string? Slug { get; set; }
        public string? Content { get; set; }
        public DateTime StartFrom { get; set; }
        public DateTime StartTo { get; set; }
        public bool Active { get; set; }
        public string Channel { get; set; } = "unknown";
        public virtual Product? Product { get; set; }
        public Adsense()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
