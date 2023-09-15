namespace Web.Gateway.Services.Implementations
{
    public class AdsenseService : IAdsenseService, IScopedDependency
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly ILogger<AdsenseService> _logger;
        HttpClient _client;
        public AdsenseService(IHttpClientFactory httpClientFactory, ILogger<AdsenseService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _client = _httpClientFactory.CreateClient("Catalog");
        }

        public async ValueTask<HttpResponseMessage> GetAsync(CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.Adsense();
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.Adsense(id);
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.AdsenseBySlug(slug);
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> GetByUserAsync(string userid, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.AdsenseByUser(userid);
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> PostAsync(AdsenseRequest request, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.Adsense();
            HttpResponseMessage response = await _client.PostAsJsonAsync(url, request, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> PutAsync(string id, AdsensePutRequest request, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.Adsense(id);
            HttpResponseMessage response = await _client.PutAsJsonAsync(url, request, cancellationToken);
            return response;
        }
        public async ValueTask<HttpResponseMessage> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.Adsense(id);
            HttpResponseMessage response = await _client.DeleteAsync(url, cancellationToken);
            return response;
        }

    }
}
