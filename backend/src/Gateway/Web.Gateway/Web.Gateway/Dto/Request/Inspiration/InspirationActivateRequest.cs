namespace Web.Gateway.Dto.Request.Inspiration
{
    public class InspirationActivateRequest
    {
        public int Id { get; set; }
        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }
    }
}
