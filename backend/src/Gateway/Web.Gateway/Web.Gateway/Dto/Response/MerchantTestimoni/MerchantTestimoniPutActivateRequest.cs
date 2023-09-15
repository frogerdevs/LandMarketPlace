namespace Web.Gateway.Dto.Response.MerchantTestimoni
{
    public class MerchantTestimoniPutActivateRequest
    {
        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }
    }
}
