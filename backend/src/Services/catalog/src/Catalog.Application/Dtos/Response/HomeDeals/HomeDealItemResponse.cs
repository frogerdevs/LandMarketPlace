using Catalog.Application.Dtos.Response.Products;

namespace Catalog.Application.Dtos.Response.HomeDeals
{
    public class HomeDealItemResponse
    {
        public required string Id { get; set; }
        public required string ProductId { get; set; }
        public string? ImgUrl { get; set; }
        public bool Active { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ProductItemResponse? Product { get; set; }
    }
}
