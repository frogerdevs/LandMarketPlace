namespace Web.Gateway.Services.Interfaces
{
    public interface IAdsenseService
    {
        ValueTask<HttpResponseMessage> GetAsync(CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> GetByUserAsync(string userid, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> PostAsync(AdsenseRequest request, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> PutAsync(string id, AdsensePutRequest request, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> DeleteAsync(string id, CancellationToken cancellationToken = default);

    }
}
