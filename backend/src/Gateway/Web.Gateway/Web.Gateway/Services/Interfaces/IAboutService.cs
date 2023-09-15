namespace Web.Gateway.Services.Interfaces
{
    public interface IAboutService
    {
        ValueTask<AboutResponse?> GetAboutAsync(string email, CancellationToken cancellationToken);
        ValueTask<AboutResponse?> GetAboutByUserAsync(string userid, CancellationToken cancellationToken);
        ValueTask<HttpResponseMessage> PutAboutAsync(string email, EditAboutRequest request, CancellationToken cancellationToken);

    }
}
