namespace Web.Gateway.Dto.Request.HomeInspiration
{
    public class PagingHomeInspirationForApi
    {
        [JsonPropertyName("page")]
        public int? Page { get; set; } = 1;
        [JsonPropertyName("limit")]
        public int? Limit { get; set; } = 10;
        [JsonPropertyName("thisYear")]
        public bool? ThisYear { get; set; }
        [JsonPropertyName("thisMonth")]
        public bool? ThisMonth { get; set; }
        [JsonPropertyName("thisWeek")]
        public bool? ThisWeek { get; set; }
    }
}
