using GrpcIdentity;

namespace Web.Gateway.Services.Implementations
{
    public class AboutService : IAboutService, IScopedDependency
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly ILogger<ProfileService> _logger;
        readonly HttpClient _client;
        private readonly AboutGrpc.AboutGrpcClient _aboutGrpcClient;
        public AboutService(IHttpClientFactory httpClientFactory, ILogger<ProfileService> logger,
            AboutGrpc.AboutGrpcClient aboutGrpcClient)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _client = _httpClientFactory.CreateClient("Identity");
            _aboutGrpcClient = aboutGrpcClient;
        }
        public async ValueTask<AboutResponse?> GetAboutAsync(string email, CancellationToken cancellationToken)
        {
            var response = await _aboutGrpcClient.GetAboutByEmailAsync(new GrpcByIdRequest { Id = email }, cancellationToken: cancellationToken);
            return MaptoAbout(response);
        }

        public async ValueTask<AboutResponse?> GetAboutByUserAsync(string userid, CancellationToken cancellationToken)
        {
            var response = await _aboutGrpcClient.GetAboutByUserAsync(new GrpcByIdRequest { Id = userid }, cancellationToken: cancellationToken);
            return MaptoAbout(response);
        }

        private static AboutResponse? MaptoAbout(GrpcAboutResponse? response)
        {
            return response != null ? new AboutResponse
            {
                UserId = response.UserId,
                Email = response.Email,
                PriceFrom = response.PriceFrom,
                PriceTo = response.PriceTo,
                Description = response.Description,
                Vision = response.Vision,
                Mission = response.Mission,
                Contact = response.Contact,
                WhatsApp = response.WhatsApp,
                Facebook = response.Facebook,
                Instagram = response.Instagram,
                Twitter = response.Twitter,
                Tiktok = response.Tiktok,
                Website = response.Website,
                SellerAchievements = response.SellerAchievements.Select(responseSellerAchievement => new SellerAchievementResponse
                {
                    Title = responseSellerAchievement.Title,
                    Image = responseSellerAchievement.Image
                }).ToList(),
                SellerEvents = response.SellerEvents.Select(responseSellerEvent => new SellerEventResponse
                {
                    Title = responseSellerEvent.Title,
                    Images = responseSellerEvent.Images.Select(responseSellerEventImage => new SellerEventImageResponse
                    {
                        Image = responseSellerEventImage.Image
                    }).ToList()
                }).ToList()
            } : null;
        }

        public async ValueTask<HttpResponseMessage> PutAboutAsync(string email, EditAboutRequest request, CancellationToken cancellationToken)
        {
            var url = UrlsConfig.IdentityService.About(email);
            HttpResponseMessage response = await _client.PutAsJsonAsync(url, request, cancellationToken);
            return response;
        }
    }
}
