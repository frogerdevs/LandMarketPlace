namespace Web.Gateway.Services.Interfaces
{
    public interface IFacilityService
    {
        ValueTask<HttpResponseMessage> GetAsync(CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> PostAsync(FacilityRequest request, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> PutAsync(string id, FacilityPutRequest request, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}
