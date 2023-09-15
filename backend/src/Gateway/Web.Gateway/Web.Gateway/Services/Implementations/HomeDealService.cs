namespace Web.Gateway.Services.Implementations
{
    public class HomeDealService : IHomeDealService, IScopedDependency
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly ILogger<HomeDealService> _logger;
        HttpClient _client;
        public HomeDealService(IHttpClientFactory httpClientFactory, ILogger<HomeDealService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _client = _httpClientFactory.CreateClient("Catalog");
        }

        public async ValueTask<HttpResponseMessage> GetAsync(GetHomeDealQuery request, CancellationToken cancellationToken = default)
        {
            string? query = QueryStringBuilderExtension<GetHomeDealQuery>.BuildQueryString(request);

            var url = UrlsConfig.CatalogService.HomeDealQuery(query);
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.HomeDeal(id);
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> PostAsync(HomeDealRequest request, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.HomeDeal();
            HttpResponseMessage response = await _client.PostAsJsonAsync(url, request, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> PutAsync(string id, HomeDealPutRequest request, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.HomeDeal(id);
            HttpResponseMessage response = await _client.PutAsJsonAsync(url, request, cancellationToken);
            return response;
        }
        public async ValueTask<HttpResponseMessage> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.HomeDeal(id);
            HttpResponseMessage response = await _client.DeleteAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> ActivateAsync(string id, ActivateHomeDealRequest request, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.HomeDealActivate(id);
            HttpResponseMessage response = await _client.PutAsJsonAsync(url, request, cancellationToken);
            return response;
        }
    }
}
