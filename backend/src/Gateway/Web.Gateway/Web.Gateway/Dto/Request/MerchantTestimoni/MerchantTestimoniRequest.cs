namespace Web.Gateway.Dto.Request.MerchantTestimoni
{
    public class MerchantTestimoniRequest
    {
        [JsonPropertyName("user_id")]
        public required string UserId { get; set; }

        [JsonPropertyName("imgUrl")]
        public string? ImgUrl { get; set; }

        [JsonPropertyName("merchant_id")]
        public string? MerchantId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("rating")]
        public int Rating { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }
        [JsonPropertyName("is_active")]
        public bool Active { get; set; }

    }
}
