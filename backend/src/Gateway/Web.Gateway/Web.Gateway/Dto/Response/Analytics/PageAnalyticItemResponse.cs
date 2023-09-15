namespace Web.Gateway.Dto.Response.Analytics
{
    public class PageAnalyticItemResponse
    {
        [JsonPropertyName("page_name")]
        public string PageName { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
