namespace Web.Gateway.Dto.Request.Profiles
{
    public class EditProfileLandersRequest
    {
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string? ImageUrl { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool NewsLetter { get; set; }
    }
}
