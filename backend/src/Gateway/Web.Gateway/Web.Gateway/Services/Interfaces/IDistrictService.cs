namespace Web.Gateway.Services.Interfaces
{
    public interface IDistrictService
    {
        ValueTask<HttpResponseMessage> GetDistrictsAsync(CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> GetDistrictByIdAsync(string id, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> GetDistrictByCityAsync(string cityid, CancellationToken cancellationToken = default);
    }
}
