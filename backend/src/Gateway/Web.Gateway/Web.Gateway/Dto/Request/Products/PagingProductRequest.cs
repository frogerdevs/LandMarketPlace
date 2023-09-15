namespace Web.Gateway.Dto.Request.Products
{
    public class PagingProductRequest
    {
        public string? UserId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? ProductName { get; set; }
        public string? CategoryName { get; set; }
        public decimal? Price { get; set; } = decimal.Zero;
        public bool? Active { get; set; }
    }
}
