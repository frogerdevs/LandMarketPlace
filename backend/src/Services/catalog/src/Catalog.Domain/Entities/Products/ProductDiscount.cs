namespace Catalog.Domain.Entities.Products
{
    public class ProductDiscount : BaseAuditableEntity<string>
    {
        public required string UserId { get; set; }
        public required string ProductId { get; set; }
        public string? DiscountName { get; set; }
        public string? Slug { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountPrice { get; set; }
        public DateTime DiscountStart { get; set; }
        public DateTime DiscountEnd { get; set; }
        public bool Active { get; set; }
        public string Channel { get; set; } = "unknown";
        public virtual Product? Product { get; set; }
        public ProductDiscount()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
