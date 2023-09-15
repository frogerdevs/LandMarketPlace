namespace Web.Gateway.Controllers.v1
{

    [ApiVersion("1.0")]
    public class AdsenseController : BaseApiController<AdsenseController>
    {
        private readonly ILogger<AdsenseController> _logger;
        private readonly IAdsenseService _adsenseService;
        public AdsenseController(ILogger<AdsenseController> logger, IAdsenseService adsenseService)
        {
            _logger = logger;
            _adsenseService = adsenseService;
        }
        // GET: api/<AdsenseController>
        // <All ProductDiscount> => For Admin
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Adsense");
            var httpResponse = await _adsenseService.GetAsync(cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // GET api/<AdsenseController>/5
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Adsense by Id");
            var httpResponse = await _adsenseService.GetByIdAsync(id, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // POST api/<AdsenseController>
        [HttpPost]
        public async Task<IActionResult> Post(AdsenseRequest request, CancellationToken cancellationToken)
        {
            var httpResponse = await _adsenseService.PostAsync(request, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // PUT api/<AdsenseController>/5
        [HttpPut("{id}")]
        public async ValueTask<IActionResult> Put(string id, [FromBody] AdsensePutRequest request, CancellationToken cancellationToken)
        {
            var httpResponse = await _adsenseService.PutAsync(id, request, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
        // DELETE api/<AdsenseController>/5
        [HttpDelete("{id}")]
        public async ValueTask<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var httpResponse = await _adsenseService.DeleteAsync(id, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
    }

}
