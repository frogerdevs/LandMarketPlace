namespace Catalog.Domain.Entities.Products
{
    public class ProductNearItem : BaseAuditableEntity<string>
    {
        public required string ProductId { get; set; }
        public required string ProductNearId { get; set; }
        public string? Title { get; set; }
        public virtual ProductNear? ProductNear { get; set; }
        public ProductNearItem()
        {
            Id = Guid.NewGuid().ToString();

        }

    }
}
