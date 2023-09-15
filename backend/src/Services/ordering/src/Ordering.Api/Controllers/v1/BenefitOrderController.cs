namespace Ordering.Api.Controllers.v1
{
    [ApiVersion("1.0")]

    public class BenefitOrderController : BaseApiController<BenefitOrderController>
    {
        private readonly ILogger<BenefitOrderController> _logger;
        public BenefitOrderController(ILogger<BenefitOrderController> logger)
        {
            _logger = logger;
        }
    }

}
