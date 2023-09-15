namespace Web.Gateway.Dto.Request.InHits
{
    public class InhitsActivateRequest
    {
        public int Id { get; set; }
        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }
    }
}
