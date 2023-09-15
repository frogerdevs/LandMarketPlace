namespace Web.Gateway.Dto.Request.EditorPick
{
    public class EditorPickRequest
    {
        [JsonPropertyName("inspiration_id")]
        public int InspirationId { get; set; }
    }
    public class EditorPickPutRequest : EditorPickRequest
    {
        public int Id { get; set; }
    }
}
