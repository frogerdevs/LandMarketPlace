namespace Web.Gateway.Dto.Response.Users
{
    public class UserItemResponse
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public bool Active { get; set; }
        public bool IsCompany { get; set; }

    }
}
