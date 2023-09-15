namespace Web.Gateway.Dto.Request.Products
{
    public class PagingBySlugRequest : BasePagingRequest
    {
        public required string Slug { get; set; }
    }
}
