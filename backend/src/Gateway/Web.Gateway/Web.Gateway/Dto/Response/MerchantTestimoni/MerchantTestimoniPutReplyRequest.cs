namespace Web.Gateway.Dto.Response.MerchantTestimoni
{
    public class MerchantTestimoniPutReplyRequest
    {
        [JsonPropertyName("message_reply")]
        public required string MessageReply { get; set; }

    }
}
