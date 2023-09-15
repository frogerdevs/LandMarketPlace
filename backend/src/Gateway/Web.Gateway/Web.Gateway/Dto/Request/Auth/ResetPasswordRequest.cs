namespace Web.Gateway.Dto.Request.Auth
{
    public class ResetPasswordRequest : BaseRequest
    {
        public string Code { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
