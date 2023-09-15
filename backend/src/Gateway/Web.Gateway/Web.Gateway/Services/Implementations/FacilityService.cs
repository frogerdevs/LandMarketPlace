namespace Web.Gateway.Services.Implementations
{
    public class FacilityService : IFacilityService, IScopedDependency
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly HttpClient _client;
        public FacilityService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _client = _httpClientFactory.CreateClient("Catalog");
        }
        public async ValueTask<HttpResponseMessage> GetAsync(CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.Facility();
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.Facility(id);
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> PostAsync(FacilityRequest request, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.Facility();
            HttpResponseMessage response = await _client.PostAsJsonAsync(url, request, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> PutAsync(string id, FacilityPutRequest request, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.Facility(id);
            HttpResponseMessage response = await _client.PutAsJsonAsync(url, request, cancellationToken);
            return response;
        }
        public async ValueTask<HttpResponseMessage> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.Facility(id);
            HttpResponseMessage response = await _client.DeleteAsync(url, cancellationToken);
            return response;
        }
    }
}
