namespace Catalog.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class HomeDealController : BaseApiController<HomeDealController>
    {
        private readonly ILogger<HomeDealController> _logger;
        public HomeDealController(ILogger<HomeDealController> logger)
        {
            _logger = logger;
        }
        // GET: api/<HomeDealController>
        [HttpGet]
        public async ValueTask<ActionResult> Get([FromQuery] GetHomeDealsQuery request, CancellationToken cancellationToken)
        {
            var items = await Mediator.Send(request, cancellationToken);
            var res = new BaseWithDataCountResponse
            {
                Success = true,
                Message = "Success Get Data",
                Count = items.Count(),
                Data = items
            };
            _logger.LogInformation("Get HomeDeal : {items}", items);
            return Ok(res);
        }

        // GET api/<HomeDealController>/5
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            var items = await Mediator.Send(new GetHomeDealById() { Id = id }, cancellationToken);
            _logger.LogInformation("Get HomeDeal by Id : {items}", items);
            if (items == null)
            {
                return NotFound();
            }
            return Ok(items);
        }

        // POST api/<HomeDealController>
        [HttpPost]
        public async Task<IActionResult> Post(AddHomeDealCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.ProductId))
            {
                return BadRequest(new ErrorResponse { Success = false, Message = "ProductId must be filled" });
            }

            return Ok(await Mediator.Send(command, cancellationToken));
        }

        // PUT api/<HomeDealController>/5
        [HttpPut("{id}")]
        public async ValueTask<IActionResult> Put(string id, [FromBody] EditHomeDealCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
            {
                return BadRequest(new ErrorResponse { Success = false, Message = "Id must equal with body" });
            }
            if (string.IsNullOrEmpty(command.ProductId))
            {
                return BadRequest(new ErrorResponse { Success = false, Message = "ProductId must be filled" });
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

        [HttpPut("[action]/{id}")]
        public async ValueTask<IActionResult> Activate(string id, [FromBody] ActivateHomeDealCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
            {
                return BadRequest(new ErrorResponse { Success = false, Message = "Id must equal with body" });
            }
            if (string.IsNullOrEmpty(command.Id))
            {
                return BadRequest(new ErrorResponse { Success = false, Message = "Id must be filled" });
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
        // DELETE api/<HomeDealController>/5
        [HttpDelete("{id}")]
        public async ValueTask<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            if (id.IsNullOrEmpty())
            {
                return BadRequest(new ErrorResponse { Success = false, Message = "Id must be filled" });
            }
            var res = await Mediator.Send(new DeleteHomeDealCommand() { Id = id }, cancellationToken);
            if (!res)
                return NotFound();

            return NoContent();
        }
    }
}
