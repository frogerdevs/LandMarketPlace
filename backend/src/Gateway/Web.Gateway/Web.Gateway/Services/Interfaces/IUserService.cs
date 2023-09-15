using Web.Gateway.Dto.Request.Users;

namespace Web.Gateway.Services.Interfaces
{
    public interface IUserService
    {
        ValueTask<HttpResponseMessage> GetUsersAsync(CancellationToken cancellationToken);
        ValueTask<HttpResponseMessage> GetByIdAsync(string id, CancellationToken cancellationToken);
        ValueTask<HttpResponseMessage> GetByEmailAsync(string email, CancellationToken cancellationToken);
        ValueTask<HttpResponseMessage> IsRegisteredAsync(string emailorphone, CancellationToken cancellationToken);
        ValueTask<HttpResponseMessage> PostAsync(UserItemRequest request, CancellationToken cancellationToken);
        ValueTask<HttpResponseMessage> PutAsync(string emailorphone, UserItemRequest request, CancellationToken cancellationToken);
        ValueTask<HttpResponseMessage> DeleteAsync(string emailorphone, CancellationToken cancellationToken);

    }
}
