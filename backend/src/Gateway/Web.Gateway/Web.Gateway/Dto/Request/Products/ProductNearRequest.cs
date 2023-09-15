namespace Web.Gateway.Dto.Request.Products
{
    public class ProductNearRequest
    {
        public string? Title { get; set; }
        public virtual ICollection<ProductNearItemRequest>? ProductNearItems { get; set; }
    }
    public class ProductNearItemRequest
    {
        public string? Title { get; set; }
    }
}
