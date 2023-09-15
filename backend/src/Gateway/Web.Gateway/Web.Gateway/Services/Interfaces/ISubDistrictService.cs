namespace Web.Gateway.Services.Interfaces
{
    public interface ISubDistrictService
    {
        ValueTask<HttpResponseMessage> GetSubDistrictsAsync(CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> GetSubDistrictByIdAsync(string id, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> GetSubDistrictByDistrictAsync(string districtid, CancellationToken cancellationToken = default);
    }
}
