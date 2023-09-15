namespace Web.Gateway.Dto.Request.HomeDeal
{
    public class ActivateHomeDealRequest
    {
        public required string Id { get; set; }
        public bool Active { get; set; }

    }
}
