namespace Web.Gateway.Services.Implementations
{
    public class DistrictService : IDistrictService, IScopedDependency
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly ILogger<DistrictService> _logger;
        readonly HttpClient _client;
        public DistrictService(IHttpClientFactory httpClientFactory, ILogger<DistrictService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _client = _httpClientFactory.CreateClient("Identity");
        }

        public async ValueTask<HttpResponseMessage> GetDistrictByCityAsync(string cityid, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.IdentityService.DistrictByCityId(cityid);
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> GetDistrictByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.IdentityService.DistrictById(id);
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> GetDistrictsAsync(CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.IdentityService.District();
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response!;
        }
    }
}
