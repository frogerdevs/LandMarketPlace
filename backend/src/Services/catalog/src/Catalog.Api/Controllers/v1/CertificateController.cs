namespace Catalog.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class CertificateController : BaseApiController<CategoryController>
    {
        private readonly ILogger<CertificateController> _logger;
        public CertificateController(ILogger<CertificateController> logger)
        {
            _logger = logger;
        }
        // GET: api/<CertificateController>
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Category");
            var items = await Mediator.Send(new GetCertificatesQuery(), cancellationToken);
            return Ok(items);
        }

        // GET api/<CertificateController>/5
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Category by Id");
            var items = await Mediator.Send(new GetCertificateByIdQuery() { Id = id }, cancellationToken);
            if (items == null)
            {
                return NotFound();
            }
            return Ok(items);
        }

        // POST api/<CertificateController>
        [HttpPost]
        public async Task<IActionResult> Post(AddCertificateCommand command, CancellationToken cancellationToken)
        {
            if (command.Name.IsNullOrEmpty())
            {
                return BadRequest(new ErrorResponse { Success = false, Message = "Name must be filled" });
            }

            return Ok(await Mediator.Send(command, cancellationToken));
        }

        // PUT api/<CertificateController>/5
        [HttpPut("{id}")]
        public async ValueTask<IActionResult> Put(string id, [FromBody] EditCertificateCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
            {
                return BadRequest(new ErrorResponse { Success = false, Message = "Id must equal with body" });
            }
            if (command.Name.IsNullOrEmpty())
            {
                return BadRequest(new ErrorResponse { Success = false, Message = "Name must be filled" });
            }
            try
            {
                var res = await Mediator.Send(command, cancellationToken);
                if (res == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Failed update data {Message}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed update data");
            }

        }

        // DELETE api/<CertificateController>/5
        [HttpDelete("{id}")]
        public async ValueTask<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            if (id.IsNullOrEmpty())
            {
                return BadRequest(new ErrorResponse { Success = false, Message = "Id must be filled" });
            }
            var res = await Mediator.Send(new DeleteCertificateCommand() { Id = id }, cancellationToken);
            if (!res)
                return NotFound();

            return NoContent();
        }
    }

}
