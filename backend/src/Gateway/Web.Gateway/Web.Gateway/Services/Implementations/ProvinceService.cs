namespace Web.Gateway.Services.Implementations
{
    public class ProvinceService : IProvinceService, IScopedDependency
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly ILogger<ProvinceService> _logger;
        readonly HttpClient _client;
        public ProvinceService(IHttpClientFactory httpClientFactory, ILogger<ProvinceService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _client = _httpClientFactory.CreateClient("Identity");
        }
        public async ValueTask<HttpResponseMessage> GetProvinciesAsync(CancellationToken cancellation = default)
        {
            var url = UrlsConfig.IdentityService.Province();
            HttpResponseMessage response = await _client.GetAsync(url, cancellation);
            return response;
        }

        public async ValueTask<HttpResponseMessage> GetProvinceByIdAsync(string id, CancellationToken cancellation = default)
        {
            var url = UrlsConfig.IdentityService.ProvinceById(id);
            HttpResponseMessage response = await _client.GetAsync(url, cancellation);
            return response;
        }
    }

}
