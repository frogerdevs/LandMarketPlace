namespace Web.Gateway.Controllers.v1
{
    [ApiVersion("1.0")]
    public class FacilityController : BaseApiController<FacilityController>
    {
        private readonly ILogger<FacilityController> _logger;
        private readonly IFacilityService _facilityService;
        public FacilityController(ILogger<FacilityController> logger, IFacilityService facilityService)
        {
            _logger = logger;
            _facilityService = facilityService;
        }
        // GET: api/<FacilityController>
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Certificate");
            var httpResponse = await _facilityService.GetAsync(cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // GET api/<FacilityController>/5
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Certificate by Id");
            var httpResponse = await _facilityService.GetByIdAsync(id, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
        // POST api/<FacilityController>
        [HttpPost]
        public async Task<IActionResult> Post(FacilityRequest request, CancellationToken cancellationToken)
        {
            var httpResponse = await _facilityService.PostAsync(request, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // PUT api/<FacilityController>/5
        [HttpPut("{id}")]
        public async ValueTask<IActionResult> Put(string id, [FromBody] FacilityPutRequest request, CancellationToken cancellationToken)
        {
            var httpResponse = await _facilityService.PutAsync(id, request, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // DELETE api/<FacilityController>/5
        [HttpDelete("{id}")]
        public async ValueTask<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var httpResponse = await _facilityService.DeleteAsync(id, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
    }

}
