namespace Web.Gateway.Dto.Request.Base
{
    public class BaseRequest
    {
    }
    public class BasePagingRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    public class BasePagingBySlugRequest : BasePagingRequest
    {
        public required string Slug { get; set; }
    }
    public class BasePagingSearchRequest : BasePagingRequest
    {
        public string? Search { get; set; }
    }
}
