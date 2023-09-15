namespace Web.Gateway.Dto.Response.MerchantTestimoni
{
    public class MerchantTestimoniesResponse : BasePagingResponse
    {
        [JsonPropertyOrder(4)]
        public new IEnumerable<MerchantTestimoniItemResponse>? Data { get; set; }

    }
}
