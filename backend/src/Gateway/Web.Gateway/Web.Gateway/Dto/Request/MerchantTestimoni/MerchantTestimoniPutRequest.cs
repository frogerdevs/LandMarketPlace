namespace Web.Gateway.Dto.Request.MerchantTestimoni
{
    public class MerchantTestimoniPutRequest : MerchantTestimoniRequest
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
