namespace Web.Gateway.Services.Interfaces
{
    public interface IOtpService
    {
        ValueTask<HttpResponseMessage> SendOtpAsync(OtpRequest request, CancellationToken cancellationToken);
        ValueTask<HttpResponseMessage> VerifyOtpAsync(VerifyOtpRequest request, CancellationToken cancellationToken);
    }
}
