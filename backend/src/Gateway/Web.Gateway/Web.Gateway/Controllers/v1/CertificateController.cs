namespace Web.Gateway.Controllers.v1
{
    [ApiVersion("1.0")]
    public class CertificateController : BaseApiController<CertificateController>
    {
        private readonly ILogger<SubCategoryController> _logger;
        private readonly ICertificateService _certificateService;
        public CertificateController(ILogger<SubCategoryController> logger, ICertificateService certificateService)
        {
            _logger = logger;
            _certificateService = certificateService;
        }
        // GET: api/<CertificateController>
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Certificate");
            var httpResponse = await _certificateService.GetAsync(cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // GET api/<CertificateController>/5
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Certificate by Id");
            var httpResponse = await _certificateService.GetByIdAsync(id, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
        // POST api/<CertificateController>
        [HttpPost]
        public async Task<IActionResult> Post(CertificatesRequest request, CancellationToken cancellationToken)
        {
            var httpResponse = await _certificateService.PostAsync(request, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // PUT api/<CertificateController>/5
        [HttpPut("{id}")]
        public async ValueTask<IActionResult> Put(string id, [FromBody] CertificatesPutRequest request, CancellationToken cancellationToken)
        {
            var httpResponse = await _certificateService.PutAsync(id, request, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // DELETE api/<CertificateController>/5
        [HttpDelete("{id}")]
        public async ValueTask<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var httpResponse = await _certificateService.DeleteAsync(id, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
    }

}
