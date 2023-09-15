namespace Web.Gateway.Services.Interfaces
{
    public interface IHomeDealService
    {
        ValueTask<HttpResponseMessage> GetAsync(GetHomeDealQuery request, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> PostAsync(HomeDealRequest request, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> PutAsync(string id, HomeDealPutRequest request, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> ActivateAsync(string id, ActivateHomeDealRequest request, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> DeleteAsync(string id, CancellationToken cancellationToken = default);

    }
}
