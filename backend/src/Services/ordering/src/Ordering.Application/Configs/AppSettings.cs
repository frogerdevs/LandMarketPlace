namespace Ordering.Application.Configs
{
    public class AppSettings
    {
        public string? AppName { get; set; }
        public Urls? Urls { get; set; }
        public string? OAuthSwaggerClientId { get; set; }
        public string? OAuthSwaggerClientSecret { get; set; }
        public List<string>? OAuthSwaggerScopes { get; set; }
        public string? UrlAuthority { get; set; }
        public string? ResourceClientId { get; set; }
        public string? ResourceClientSecret { get; set; }
        public string? UrlIssuer { get; set; }
        public string? BrokerHostName { get; set; }
        public int BrokerPort { get; set; }
        public string? BrokerUserName { get; set; }
        public string? BrokerPassword { get; set; }
        public string? BrokerName { get; set; }
        public string? QueueName { get; set; }

    }
    public class Urls
    {
        public string? Identity { get; set; }
        public string? Core { get; set; }
    }
}
