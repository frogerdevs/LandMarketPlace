namespace Web.Gateway.Dto.Response.Deals
{
    public class HomeDealsResponse : BaseWithDataCountResponse
    {
        [JsonPropertyOrder(4)]
        public new IEnumerable<HomeDealsItem>? Data { get; set; }

    }
}
