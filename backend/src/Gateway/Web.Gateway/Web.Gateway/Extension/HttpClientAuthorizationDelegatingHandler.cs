using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace Web.Gateway.Extension
{
    public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpClientAuthorizationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authorizationHeader = _httpContextAccessor.HttpContext!
                .Request.Headers["Authorization"];
            var existauth = request.Headers.FirstOrDefault(c => c.Key == "Authorization").Key;

            if (!string.IsNullOrEmpty(authorizationHeader) && string.IsNullOrEmpty(existauth))
            {
                request.Headers.Add("Authorization", new List<string>() { authorizationHeader! });
            }

            var token = await GetToken();

            if (token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }

        async Task<string?> GetToken()
        {
            const string ACCESS_TOKEN = "access_token";

            return await _httpContextAccessor.HttpContext!
                .GetTokenAsync(ACCESS_TOKEN);
        }
    }
}
