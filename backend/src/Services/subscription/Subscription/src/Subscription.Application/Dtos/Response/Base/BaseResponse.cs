namespace Subscription.Application.Dtos.Response.Base
{
    public class BaseResponse
    {
        [JsonPropertyOrder(1)]
        public bool Success { get; set; }
        [JsonPropertyOrder(2)]
        public string? Message { get; set; }
    }
    public class BaseWithDataResponse : BaseResponse
    {
        [JsonPropertyOrder(3)]
        public int Count { get; set; }
        [JsonPropertyOrder(4)]
        public object? Data { get; set; }
    }
    public class BasePagingResponse : BaseResponse
    {
        [JsonPropertyOrder(3)]
        public int Count { get; set; }
        public int TotalData { get; set; }
        public int Limit { get; set; }
        public int CurrentPage { get; set; }
        [JsonPropertyOrder(4)]
        public object? Data { get; set; }
    }
}