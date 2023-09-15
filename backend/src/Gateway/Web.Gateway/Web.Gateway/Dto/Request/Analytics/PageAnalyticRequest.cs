namespace Web.Gateway.Dto.Request.Analytics
{
    public class PageAnalyticRequest
    {
        [JsonPropertyName("page_name")]
        public string? PageName { get; set; }
    }
    public class PageAnalyticPutRequest : PageAnalyticRequest
    {
        public int Id { get; set; }
    }
}
