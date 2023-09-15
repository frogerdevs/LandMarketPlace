namespace Catalog.Application.Dtos.Response.Base;

public class BaseResponse
{
    [JsonPropertyOrder(1)]
    public bool Success { get; set; }
    [JsonPropertyOrder(2)]
    public string? Message { get; set; }
}
public class BaseWithDataResponse : BaseResponse
{
    [JsonPropertyOrder(4)]
    public object? Data { get; set; }
}
public class BaseWithDataResponse<T> : BaseResponse
{
    [JsonPropertyOrder(4)]
    public T? Data { get; set; }
}
public class BaseWithDataCountResponse : BaseResponse
{
    [JsonPropertyOrder(3)]
    public int Count { get; set; }
    [JsonPropertyOrder(4)]
    public object? Data { get; set; }
}
public class BaseWithDataCountResponse<T> : BaseResponse
{
    [JsonPropertyOrder(3)]
    public int Count { get; set; }
    [JsonPropertyOrder(4)]
    public IEnumerable<T>? Data { get; set; }
}
public class BasePagingResponse : BaseResponse
{
    [JsonPropertyOrder(3)]
    public int Count { get; set; }
    [JsonPropertyName("total_data")]
    public int TotalData { get; set; }
    public int Limit { get; set; }
    [JsonPropertyName("current_page")]
    public int CurrentPage { get; set; }
    [JsonPropertyOrder(4)]
    public object? Data { get; set; }
}
public class BasePagingResponse<T> : BaseResponse
{
    [JsonPropertyOrder(3)]
    public int Count { get; set; }
    [JsonPropertyName("total_data")]
    public int TotalData { get; set; }
    public int Limit { get; set; }
    [JsonPropertyName("current_page")]
    public int CurrentPage { get; set; }
    [JsonPropertyOrder(4)]
    public IEnumerable<T>? Data { get; set; }
}
