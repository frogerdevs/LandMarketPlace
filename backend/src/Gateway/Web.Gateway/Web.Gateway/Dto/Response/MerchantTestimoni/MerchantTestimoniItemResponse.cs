namespace Web.Gateway.Dto.Response.MerchantTestimoni
{
    public class MerchantTestimoniItemResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("user_id")]
        public required string UserId { get; set; }

        [JsonPropertyName("merchant_id")]
        public string? MerchantId { get; set; }

        [JsonPropertyName("imgUrl")]
        public string? ImgUrl { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("rating")]
        public int Rating { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("message_reply")]
        public string? MessageReply { get; set; }

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }

        [JsonPropertyName("datePublish")]
        public string? DatePublish { get; set; }
    }
}
