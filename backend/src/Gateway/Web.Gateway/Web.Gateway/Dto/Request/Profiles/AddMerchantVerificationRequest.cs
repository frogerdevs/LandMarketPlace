namespace Web.Gateway.Dto.Request.Profiles
{
    public class AddMerchantVerificationRequest
    {
        public required string Email { get; set; }
        public string? NpwpFile { get; set; }
        public string? KtpFile { get; set; }
        public string? SuratKuasaFile { get; set; }
    }
}
