namespace Web.Gateway.Dto.Response.Inspiration
{
    public class InspirationCommentItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("user_id")]
        public required string UserId { get; set; }

        [JsonPropertyName("inspiration_id")]
        public int InspirationId { get; set; }

        [JsonPropertyName("content")]
        public string? Content { get; set; }

        [JsonPropertyName("created_at")]
        public string? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public string? UpdatedAt { get; set; }

        [JsonPropertyName("replies")]
        public List<InspirationCommentItem>? Replies { get; set; } = new List<InspirationCommentItem>();
    }
}
