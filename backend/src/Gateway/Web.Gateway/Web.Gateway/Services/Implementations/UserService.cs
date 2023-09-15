using System.Text.Json;
using Web.Gateway.Config;
using Web.Gateway.Dto.Request.Users;
using Web.Gateway.Services.Interfaces;
using Web.Gateway.Services.Interfaces.Base;

namespace Web.Gateway.Services.Implementations
{
    public class UserService : IUserService, IScopedDependency
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly ILogger<UserService> _logger;
        readonly HttpClient _client;
        public UserService(IHttpClientFactory httpClientFactory, ILogger<UserService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _client = _httpClientFactory.CreateClient("Identity");
        }

        public async ValueTask<HttpResponseMessage> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var url = UrlsConfig.IdentityService.UserByEmail(email);
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var url = UrlsConfig.IdentityService.UserById(id);
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> GetUsersAsync(CancellationToken cancellationToken)
        {
            var url = UrlsConfig.IdentityService.User();
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> IsRegisteredAsync(string emailorphone, CancellationToken cancellationToken)
        {
            var url = UrlsConfig.IdentityService.UserIsRegistered(emailorphone);
            HttpResponseMessage response = await _client.GetAsync(url, cancellationToken);
            return response;
        }

        public async ValueTask<HttpResponseMessage> PostAsync(UserItemRequest request, CancellationToken cancellationToken)
        {
            var url = UrlsConfig.IdentityService.User();
            _logger.LogInformation($"Post User: {JsonSerializer.Serialize(request)}");
            return await _client.PostAsJsonAsync(url, request, cancellationToken);
        }

        public async ValueTask<HttpResponseMessage> PutAsync(string emailorphone, UserItemRequest request, CancellationToken cancellationToken)
        {
            var url = UrlsConfig.IdentityService.UserById(emailorphone);
            _logger.LogInformation($"PutAsync User: {JsonSerializer.Serialize(request)}");
            return await _client.PutAsJsonAsync(url, request, cancellationToken);
        }
        public async ValueTask<HttpResponseMessage> DeleteAsync(string emailorphone, CancellationToken cancellationToken)
        {
            var url = UrlsConfig.IdentityService.UserById(emailorphone);
            _logger.LogInformation($"DeleteAsync User: {JsonSerializer.Serialize(emailorphone)}");
            return await _client.DeleteAsync(url, cancellationToken);
        }
    }
}
