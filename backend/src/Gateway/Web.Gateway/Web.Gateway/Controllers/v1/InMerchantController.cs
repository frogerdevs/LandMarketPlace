namespace Web.Gateway.Controllers.v1
{
    [ApiVersion("1.0")]
    public class InMerchantController : BaseApiController<InMerchantController>
    {
        readonly IInMerchantService _inMerchantService;
        public InMerchantController(IInMerchantService inMerchantService)
        {
            _inMerchantService = inMerchantService;
        }
        [HttpGet("[action]")]
        public async ValueTask<ActionResult> ByCategorySlug([FromQuery] BasePagingBySlugRequest request, CancellationToken cancellationToken)
        {
            Logger.LogInformation("Get Product by category slug");
            var response = await _inMerchantService.GetByCategorySlugAsync(request, cancellationToken);
            return Ok(response);
        }

    }
}
