namespace Catalog.Domain.Entities.Products
{
    public class ProductNear : BaseAuditableEntity<string>
    {
        public required string ProductId { get; set; }
        public string? Title { get; set; }
        public virtual ICollection<ProductNearItem>? ProductNearItems { get; set; }
        public virtual Product? Product { get; set; }
        public ProductNear()
        {
            Id = Guid.NewGuid().ToString();

        }
    }
}
