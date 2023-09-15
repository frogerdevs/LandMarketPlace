namespace Web.Gateway.Dto.Response.HomeInspiration
{
    public class HomeInspirationsResponse : BasePagingResponse
    {
        [JsonPropertyOrder(4)]
        public new IEnumerable<HomeInspirationListItem>? Data { get; set; }
    }
}
