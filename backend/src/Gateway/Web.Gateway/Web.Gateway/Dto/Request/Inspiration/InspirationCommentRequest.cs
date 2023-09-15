namespace Web.Gateway.Dto.Request.Inspiration
{
    public class InspirationCommentRequest
    {
        [JsonPropertyName("user_id")]
        public required string UserId { get; set; }

        [JsonPropertyName("inspiration_id")]
        public int InspirationId { get; set; }

        [JsonPropertyName("content")]
        public required string Content { get; set; }
    }
}
