namespace Web.Gateway.Config
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
        public string? Catalog { get; set; }
        public string? Inspiration { get; set; }
        public string? Exhibition { get; set; }
        public string? Testimonial { get; set; }
        public string? Analytic { get; set; }
        public string? GrpcCatalog { get; set; }
        public string? GrpcSubscription { get; set; }
        public string? GrpcOrdering { get; set; }
        public string? GrpcIdentity { get; set; }
    }
}
