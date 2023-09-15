namespace Web.Gateway.Dto.Request.Auth
{
    public class ForgotPasswordRequest : BaseRequest
    {
        public string Email { get; set; }
        public string HostUrl { get; set; }
    }
}
