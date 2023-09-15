namespace Web.Gateway.Dto.Response.Inspiration
{
    public class InspirationsResponse : BasePagingResponse
    {
        [JsonPropertyOrder(4)]
        public new IEnumerable<InspirationListItem>? Data { get; set; }
    }
}
