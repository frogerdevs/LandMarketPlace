namespace IdentityServer.Dtos.Requests.Otps
{
    public class OtpRequest
    {
        public string EmailOrPhone { get; set; }
    }
    public class VerifyOtpRequest : OtpRequest
    {
        public string OtpCode { get; set; }
    }
}
