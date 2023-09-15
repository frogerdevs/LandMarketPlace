namespace Web.Gateway.Controllers.v1
{
    [ApiVersion("1.0")]
    public class HomeDealController : BaseApiController<HomeDealController>
    {
        private readonly ILogger<HomeDealController> _logger;
        private readonly IHomeDealService _homeDealService;
        public HomeDealController(ILogger<HomeDealController> logger, IHomeDealService homeDealService)
        {
            _logger = logger;
            _homeDealService = homeDealService;
        }
        // GET: api/<HomeDealController>
        // <All ProductDiscount> => For Admin
        [HttpGet]
        public async ValueTask<ActionResult> Get([FromQuery] GetHomeDealQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Adsense");
            var httpResponse = await _homeDealService.GetAsync(request, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // GET api/<HomeDealController>/5
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Adsense by Id");
            var httpResponse = await _homeDealService.GetByIdAsync(id, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // POST api/<HomeDealController>
        [HttpPost]
        public async Task<IActionResult> Post(HomeDealRequest request, CancellationToken cancellationToken)
        {
            var httpResponse = await _homeDealService.PostAsync(request, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // PUT api/<HomeDealController>/5
        [HttpPut("{id}")]
        public async ValueTask<IActionResult> Put(string id, [FromBody] HomeDealPutRequest request, CancellationToken cancellationToken)
        {
            var httpResponse = await _homeDealService.PutAsync(id, request, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        [HttpPut("[action]/{id}")]
        public async ValueTask<IActionResult> Activate(string id, [FromBody] ActivateHomeDealRequest request, CancellationToken cancellationToken)
        {
            var httpResponse = await _homeDealService.ActivateAsync(id, request, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
        // DELETE api/<HomeDealController>/5
        [HttpDelete("{id}")]
        public async ValueTask<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var httpResponse = await _homeDealService.DeleteAsync(id, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
    }

}
