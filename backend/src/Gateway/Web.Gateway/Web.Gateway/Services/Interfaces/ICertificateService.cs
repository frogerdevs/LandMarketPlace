namespace Web.Gateway.Services.Interfaces
{
    public interface ICertificateService
    {
        ValueTask<HttpResponseMessage> GetAsync(CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> PostAsync(CertificatesRequest request, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> PutAsync(string id, CertificatesPutRequest request, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}
