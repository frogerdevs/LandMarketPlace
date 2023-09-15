namespace Web.Gateway.Dto.Request.HomeInspiration
{
    public class GetPagingHomeInspirationRequest
    {
        public int? PageNumber { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
        public bool? ThisYear { get; set; } = false;
        public bool? ThisMonth { get; set; } = false;
        public bool? ThisWeek { get; set; } = false;
    }
}
