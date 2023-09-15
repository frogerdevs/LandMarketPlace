using GrpcCatalog;
using GrpcIdentity;

namespace Web.Gateway.Services.Implementations
{
    public class ProfileService : IProfileService, IScopedDependency
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly ILogger<ProfileService> _logger;
        readonly HttpClient _client;
        readonly HttpClient _clientCatalog;
        private readonly UserGrpc.UserGrpcClient _userGrpcClient;
        private readonly CategoryGrpc.CategoryGrpcClient _categoryGrpcClient;
        public ProfileService(IHttpClientFactory httpClientFactory, ILogger<ProfileService> logger,
            UserGrpc.UserGrpcClient userGrpcClient, CategoryGrpc.CategoryGrpcClient categoryGrpcClient)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _client = _httpClientFactory.CreateClient("Identity");
            _clientCatalog = _httpClientFactory.CreateClient("Catalog");
            _userGrpcClient = userGrpcClient;
            _categoryGrpcClient = categoryGrpcClient;
        }
        public async ValueTask<HttpResponseMessage> GetLandersAsync(string email, CancellationToken cancellationToken)
        {
            var url = UrlsConfig.IdentityService.ProfileLanders(email);
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<ProfileMerchantResponse?> GetMerchantAsync(string email, CancellationToken cancellationToken)
        {
            var url = UrlsConfig.IdentityService.ProfileMerchant(email);
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var profile = await response.Content.ReadFromJsonAsync<ProfileMerchantResponse>(cancellationToken: cancellationToken);
                if (profile?.Data?.CategoryId != null)
                {
                    var urlCatalog = UrlsConfig.CatalogService.Category(profile!.Data!.CategoryId);
                    HttpResponseMessage responseCatalog = await _clientCatalog.GetAsync(urlCatalog, cancellationToken);
                    if (responseCatalog.IsSuccessStatusCode)
                    {
                        var category = await responseCatalog.Content.ReadFromJsonAsync<CategoryResponse>(cancellationToken: cancellationToken);
                        profile.Data.CategoryName = category?.Data?.Name;

                        return profile;
                    }
                }

                return profile;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            return new ProfileMerchantResponse { Success = false, Message = "Failed Get Merchant", Data = null };
        }

        public async ValueTask<HttpResponseMessage> PutLandersAsync(EditProfileLandersRequest request, CancellationToken cancellationToken)
        {
            var url = UrlsConfig.IdentityService.ProfileLanders(request.Email);
            HttpResponseMessage response = await _client.PutAsJsonAsync(url, request, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> PutMerchantAsync(EditProfileMerchantRequest request, CancellationToken cancellationToken)
        {
            var url = UrlsConfig.IdentityService.ProfileMerchant(request.Email);
            JsonContent content = JsonContent.Create(request, mediaType: null);
            HttpResponseMessage response = await _client.PutAsync(url, content, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> AddMerchantVerificationAsync(AddMerchantVerificationRequest request, CancellationToken cancellationToken)
        {
            var url = UrlsConfig.IdentityService.ProfileMerchantVerification();
            JsonContent content = JsonContent.Create(request, mediaType: null);
            HttpResponseMessage response = await _client.PostAsync(url, content, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            var url = UrlsConfig.IdentityService.ProfileChangePassword();
            JsonContent content = JsonContent.Create(request, mediaType: null);
            HttpResponseMessage response = await _client.PostAsync(url, content, cancellationToken);
            return response;
        }

        public async ValueTask<ProfileByBrandSlugResponse?> GetProfileBySlugAsync(string slug, CancellationToken cancellationToken)
        {
            var response = await _userGrpcClient.GetProfileBrandBySlugAsync(new GrpcIdentity.GrpcBySlugRequest { Slug = slug }, cancellationToken: cancellationToken);
            var result = await MaptoProfileMerchant(response);
            return result;
        }

        private async Task<ProfileByBrandSlugResponse?> MaptoProfileMerchant(GrpcProfileMerchantResponse? response)
        {
            if (response == null)
                return null;

            //var res = await _grpcClient.GetItemByIdAsync(new GrpcCatalog.GrpcByIdRequest { Id = id }, cancellationToken: cancellationToken);
            var category = await _categoryGrpcClient.GetItemByIdAsync(new GrpcCatalog.GrpcByIdRequest { Id = response.CategoryId });
            var categoryname = category.Name;
            return new ProfileByBrandSlugResponse
            {
                UserId = response.UserId,
                ImageUrl = response.ImageUrl,
                BrandName = response.BrandName,
                BrandSlug = response.BrandSlug,
                CategoryId = response.CategoryId,
                CategoryName = categoryname,
                Address = response.Address,
                Province = response.Province,
                City = response.City,
                District = response.District,
                SubDistrict = response.SubDistrict,
                PostCode = response.PostCode,
                Verified = response.Verified
            };
        }
    }
}
