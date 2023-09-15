namespace Web.Gateway.Dto.Response.Testimoni
{
    public class TestimoniesResponse : BasePagingResponse
    {
        [JsonPropertyOrder(4)]
        public new IEnumerable<TestimoniItemResponse>? Data { get; set; }

    }
}
