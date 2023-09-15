namespace Web.Gateway.Services.Implementations
{
    public class CertificateService : ICertificateService, IScopedDependency
    {
        readonly IHttpClientFactory _httpClientFactory;
        HttpClient _client;
        public CertificateService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _client = _httpClientFactory.CreateClient("Catalog");
        }
        public async ValueTask<HttpResponseMessage> GetAsync(CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.Certificate();
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.Certificate(id);
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> PostAsync(CertificatesRequest request, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.Certificate();
            HttpResponseMessage response = await _client.PostAsJsonAsync(url, request, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> PutAsync(string id, CertificatesPutRequest request, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.Certificate(id);
            HttpResponseMessage response = await _client.PutAsJsonAsync(url, request, cancellationToken);
            return response;
        }
        public async ValueTask<HttpResponseMessage> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.Certificate(id);
            HttpResponseMessage response = await _client.DeleteAsync(url, cancellationToken);
            return response;
        }
    }
}
