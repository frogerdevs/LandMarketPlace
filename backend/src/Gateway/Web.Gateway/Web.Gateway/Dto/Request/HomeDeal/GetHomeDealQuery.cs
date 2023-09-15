namespace Web.Gateway.Dto.Request.HomeDeal
{
    public class GetHomeDealQuery
    {
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 0;
        public string? Search { get; set; }

    }
}
