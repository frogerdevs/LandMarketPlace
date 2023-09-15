namespace Web.Gateway.Services.Interfaces
{
    public interface IProvinceService
    {
        ValueTask<HttpResponseMessage> GetProvinciesAsync(CancellationToken cancellation = default);
        ValueTask<HttpResponseMessage> GetProvinceByIdAsync(string id, CancellationToken cancellation = default);
    }
}
