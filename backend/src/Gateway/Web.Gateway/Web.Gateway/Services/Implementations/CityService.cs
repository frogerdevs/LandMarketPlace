namespace Web.Gateway.Services.Implementations
{
    public class CityService : ICityService, IScopedDependency
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly ILogger<CityService> _logger;
        readonly HttpClient _client;
        public CityService(IHttpClientFactory httpClientFactory, ILogger<CityService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _client = _httpClientFactory.CreateClient("Identity");
        }
        public async ValueTask<HttpResponseMessage> GetCitiesAsync(CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.IdentityService.City();
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response!;
        }

        public async ValueTask<HttpResponseMessage> GetCityByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.IdentityService.CityById(id);
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> GetCityByProvinceAsync(string provinceid, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.IdentityService.CityByProvinceId(provinceid);
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }
    }
}
