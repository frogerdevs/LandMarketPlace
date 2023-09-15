namespace Web.Gateway.Services.Implementations
{
    public class AuthService : IAuthService, IScopedDependency
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly ILogger<AuthService> _logger;
        readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(IHttpClientFactory httpClientFactory, ILogger<AuthService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _client = _httpClientFactory.CreateClient("Identity");
            _httpContextAccessor = httpContextAccessor;
        }

        public async ValueTask<HttpResponseMessage> GetCsrf(CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.IdentityService.GetCsrf();
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            //var headers = _httpContextAccessor.HttpContext?.Request.Headers;
            //// Menyalin header dari HttpContext ke permintaan HttpClient
            //if (headers != null)
            //{
            //    foreach (var (key, value) in headers)
            //    {
            request.Headers.TryAddWithoutValidation("asal", "masuk ajah nih");
            //    }
            //}
            //// Meneruskan permintaan ke service
            HttpResponseMessage response = await _client.SendAsync(request, cancellationToken);


            //HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> GetUserInfo(CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.IdentityService.GetUserInfo();
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var headers = _httpContextAccessor.HttpContext?.Request.Headers;
            //// Menyalin header dari HttpContext ke permintaan HttpClient
            //if (headers != null)
            //{
            //    foreach (var (key, value) in headers)
            //    {
            //request.Headers.TryAddWithoutValidation("asal", "masuk ajah nih");
            //    }
            //}
            //// Meneruskan permintaan ke service
            HttpResponseMessage response = await _client.SendAsync(request, cancellationToken);
            //HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);

            return response;
        }

        public async ValueTask<HttpResponseMessage> PostToGetToken(AuthTokenRequest request, CancellationToken cancellationToken)
        {
            var url = UrlsConfig.IdentityService.Token();
            // Mengonversi model menjadi dictionary
            var formValues = new Dictionary<string, string>
    {
        { nameof(AuthTokenRequest.client_id), request.client_id },
        { nameof(AuthTokenRequest.client_secret), request.client_secret },
        { nameof(AuthTokenRequest.grant_type), request.grant_type},
        { nameof(AuthTokenRequest.username), request.username },
        { nameof(AuthTokenRequest.password), request.password },
        { nameof(AuthTokenRequest.scope), request.scope }
        // Tambahkan properti lain yang sesuai dengan data form Anda
    };
            HttpResponseMessage response = await _client.PostAsync(url,
                new FormUrlEncodedContent(formValues), cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> Signout(Dictionary<string, string?> form, CancellationToken cancellationToken = default)
        {
            var url = UrlsConfig.IdentityService.Signout();

            HttpResponseMessage response = await _client.PostAsync(url,
                new FormUrlEncodedContent(form), cancellationToken);
            return response;
        }
        public async ValueTask<HttpResponseMessage> RegisterLanders(RegisterLandersRequest request, CancellationToken cancellationToken)
        {
            var url = UrlsConfig.IdentityService.RegisterLanders();
            HttpResponseMessage response = await _client.PostAsJsonAsync(url, request, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> RegisterMerchant(RegisterMerchantRequest request, CancellationToken cancellationToken)
        {
            var url = UrlsConfig.IdentityService.RegisterMerchant();
            HttpResponseMessage response = await _client.PostAsJsonAsync(url, request, cancellationToken);
            return response;
        }
        public async ValueTask<HttpResponseMessage> ForgotPassword(ForgotPasswordRequest request, CancellationToken cancellationToken)
        {
            var url = UrlsConfig.IdentityService.ForgotPassword();
            HttpResponseMessage response = await _client.PostAsJsonAsync(url, request, cancellationToken);
            return response;
        }
        public async ValueTask<HttpResponseMessage> ResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            var url = UrlsConfig.IdentityService.ResetPassword();
            HttpResponseMessage response = await _client.PostAsJsonAsync(url, request, cancellationToken);
            return response;
        }

    }
}
