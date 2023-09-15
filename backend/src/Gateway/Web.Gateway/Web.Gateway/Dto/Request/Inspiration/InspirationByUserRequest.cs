namespace Web.Gateway.Dto.Request.Inspiration
{
    public class InspirationByUserRequest : BasePagingSearchRequest
    {
        public string? UserId { get; set; }
    }
}
