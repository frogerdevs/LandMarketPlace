namespace Web.Gateway.Services.Implementations
{
    public class SubCategoryService : ISubCategoryService, IScopedDependency
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly ILogger<SubCategoryService> _logger;
        HttpClient _client;
        public SubCategoryService(IHttpClientFactory httpClientFactory, ILogger<SubCategoryService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _client = _httpClientFactory.CreateClient("Catalog");
        }

        public async ValueTask<HttpResponseMessage> GetAsync(CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.SubCategory();
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.SubCategory(id);
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.SubCategoryBySlug(slug);
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> PostAsync(SubCategoryRequest request, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.SubCategory();
            HttpResponseMessage response = await _client.PostAsJsonAsync(url, request, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> PutAsync(string id, SubCategoryPutRequest request, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.SubCategory(id);
            HttpResponseMessage response = await _client.PutAsJsonAsync(url, request, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.CatalogService.SubCategory(id);
            HttpResponseMessage response = await _client.DeleteAsync(url, cancellationToken);
            return response;
        }
    }
}
