namespace Web.Gateway.Services.Implementations
{
    public class SubDistrictService : ISubDistrictService, IScopedDependency
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly ILogger<SubDistrictService> _logger;
        readonly HttpClient _client;
        public SubDistrictService(IHttpClientFactory httpClientFactory, ILogger<SubDistrictService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _client = _httpClientFactory.CreateClient("Identity");
        }

        public async ValueTask<HttpResponseMessage> GetSubDistrictByDistrictAsync(string districtid, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.IdentityService.SubDistrictByDistrictId(districtid);
            HttpResponseMessage response = await _client.GetAsync(url);
            return response;
        }

        public async ValueTask<HttpResponseMessage> GetSubDistrictByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.IdentityService.SubDistrictById(id);
            HttpResponseMessage response = await _client.GetAsync(url);
            return response;
        }

        public async ValueTask<HttpResponseMessage> GetSubDistrictsAsync(CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.IdentityService.SubDistrict();
            HttpResponseMessage response = await _client.GetAsync(url);
            return response!;
        }
    }
}
