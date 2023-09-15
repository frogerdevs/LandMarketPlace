namespace Catalog.Domain.Entities.Products
{
    public class ProductImage : BaseAuditableEntity<string>
    {
        public required string ProductId { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageType { get; set; }
        public string? ImageName { get; set; }
        public virtual Product? Product { get; set; }
        public ProductImage()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
