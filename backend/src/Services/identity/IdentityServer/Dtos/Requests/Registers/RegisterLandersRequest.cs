namespace IdentityServer.Dtos.Requests.Registers
{
    public class RegisterLandersRequest : BaseRequest
    {
        public string FirstName { get; set; } = "";
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? Provider { get; set; }
        public bool EmailConfirmed { get; set; } = false;
    }
}
