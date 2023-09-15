namespace Web.Gateway.Services.Interfaces
{
    public interface IAuthService
    {
        ValueTask<HttpResponseMessage> PostToGetToken(AuthTokenRequest emailorphone, CancellationToken cancellationToken);
        ValueTask<HttpResponseMessage> Signout(Dictionary<string, string?> form, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> GetCsrf(CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> GetUserInfo(CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> RegisterMerchant(RegisterMerchantRequest request, CancellationToken cancellationToken);
        ValueTask<HttpResponseMessage> RegisterLanders(RegisterLandersRequest request, CancellationToken cancellationToken);
        ValueTask<HttpResponseMessage> ForgotPassword(ForgotPasswordRequest request, CancellationToken cancellationToken);
        ValueTask<HttpResponseMessage> ResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken);
    }
}
