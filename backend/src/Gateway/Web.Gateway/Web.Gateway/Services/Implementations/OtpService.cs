namespace Web.Gateway.Services.Implementations
{
    public class OtpService : IOtpService, IScopedDependency
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly ILogger<ProfileService> _logger;
        readonly HttpClient _client;
        public OtpService(IHttpClientFactory httpClientFactory, ILogger<ProfileService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _client = _httpClientFactory.CreateClient("Identity");
        }

        public async ValueTask<HttpResponseMessage> SendOtpAsync(OtpRequest request, CancellationToken cancellationToken)
        {
            var url = UrlsConfig.IdentityService.SendOtp();
            HttpResponseMessage response = await _client.PostAsJsonAsync(url, request, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> VerifyOtpAsync(VerifyOtpRequest request, CancellationToken cancellationToken)
        {
            var url = UrlsConfig.IdentityService.VerifyOtp();
            HttpResponseMessage response = await _client.PostAsJsonAsync(url, request, cancellationToken);
            return response;
        }
    }
}
