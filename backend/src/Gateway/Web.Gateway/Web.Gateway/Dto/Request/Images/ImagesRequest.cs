namespace Web.Gateway.Dto.Request.Images
{
    public class ImagesRequest
    {
        public string? ImageId { get; set; }
        public IFormFile? Image { get; set; }
    }
}
