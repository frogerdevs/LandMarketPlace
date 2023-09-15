namespace Web.Gateway.Dto.Request.Testimoni
{
    public class TestimoniRequest : BaseRequest
    {
        [JsonPropertyName("imgUrl")]
        public string? ImgUrl { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("rating")]
        public int Rating { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }
        [JsonPropertyName("is_active")]
        public bool? Active { get; set; }
    }
    public class TestimoniPutRequest : TestimoniRequest
    {
        public int Id { get; set; }
    }
}
