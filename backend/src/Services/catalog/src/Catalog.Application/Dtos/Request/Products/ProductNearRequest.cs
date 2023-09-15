namespace Catalog.Application.Dtos.Request.Products
{
    public class ProductNearRequest
    {
        public string? Title { get; set; }
        public virtual ICollection<ProductNearItemRequest>? ProductNearItems { get; set; }
    }
}
