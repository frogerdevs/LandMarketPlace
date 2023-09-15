namespace Web.Gateway.Services.Interfaces
{
    public interface ICityService
    {
        ValueTask<HttpResponseMessage> GetCitiesAsync(CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> GetCityByIdAsync(string id, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> GetCityByProvinceAsync(string provinceid, CancellationToken cancellationToken = default);
    }
}
