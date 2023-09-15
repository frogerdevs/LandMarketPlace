namespace Web.Gateway.Extension
{
    public class HttpClientChannelDelegatingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpClientChannelDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var channelHeader = _httpContextAccessor.HttpContext!.Request.Headers["channel"];
            if (!string.IsNullOrEmpty(channelHeader))
            {
                request.Headers.Add("channel", new List<string>() { channelHeader.ToString() });
            }
            else
            {
                request.Headers.Add("channel", "Unknown");
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
