namespace Web.Gateway.Dto.Request.HomeDeal
{
    public class HomeDealRequest
    {
        public required string ProductId { get; set; }
        public string? ImgUrl { get; set; }
        public bool Active { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
