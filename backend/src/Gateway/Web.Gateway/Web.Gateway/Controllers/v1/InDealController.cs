using Web.Gateway.Dto.Request.InDeals;

namespace Web.Gateway.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[Authorize]
    public class InDealController : ControllerBase
    {
        private readonly ILogger<InDealController> _logger;
        private readonly IInDealsService _inDealsService;
        public InDealController(ILogger<InDealController> logger, IInDealsService inDealsService)
        {
            _logger = logger;
            _inDealsService = inDealsService;
        }
        [HttpGet]
        public async ValueTask<ActionResult> Get([FromQuery] PagingInDealsRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Masuk nih guys");

            var res = await _inDealsService.GetInDealsAsync(new InDealsRequest
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Search = request.Search
            }, cancellationToken);
            //var items = await Mediator.Send(new GetAllBrandQuery(), cancellationToken);

            return Ok(res);
        }
    }
}
