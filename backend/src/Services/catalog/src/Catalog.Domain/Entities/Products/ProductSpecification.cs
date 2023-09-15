namespace Catalog.Domain.Entities.Products
{
    public class ProductSpecification : BaseAuditableEntity<string>
    {
        public required string ProductId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public virtual Product? Product { get; set; }
        public ProductSpecification()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
