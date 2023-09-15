namespace Subscription.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class UnitTypeController : BaseApiController<UnitTypeController>
    {
        private readonly ILogger<UnitTypeController> _logger;
        public UnitTypeController(ILogger<UnitTypeController> logger)
        {
            _logger = logger;
        }
        // GET: api/<UnitTypeController>
        // <All UnitType> => For Admin
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get UnitType");

            var items = await Mediator.Send(new GetUnitTypesQuery(), cancellationToken);
            var res = new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Get Data",
                Data = items
            };
            return Ok(res);
        }

        // GET api/<UnitTypeController>/5
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get UnitType by Id");
            var item = await Mediator.Send(new GetUnitTypeByIdQuery() { Id = id }, cancellationToken);
            if (item == null)
                return NotFound();

            return Ok(new BaseWithDataResponse { Success = true, Message = "Success Get Data", Data = item });
        }

        // POST api/<UnitTypeController>
        [HttpPost]
        public async Task<IActionResult> Post(AddUnitTypeCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.Name))
            {
                return BadRequest(new BaseResponse { Success = false, Message = "Name must be filled" });
            }

            return Ok(await Mediator.Send(command, cancellationToken));
        }

        // PUT api/<UnitTypeController>/5
        [HttpPut("{id}")]
        public async ValueTask<IActionResult> Put(string id, [FromBody] EditUnitTypeCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
            {
                return BadRequest(new BaseResponse { Success = false, Message = "Id must equal with body" });
            }
            if (string.IsNullOrEmpty(command.Name))
            {
                return BadRequest(new BaseResponse { Success = false, Message = "Name must be filled" });
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

        // DELETE api/<UnitTypeController>/5
        [HttpDelete("{id}")]
        public async ValueTask<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new BaseResponse { Success = false, Message = "Id must be filled" });
            }
            var res = await Mediator.Send(new DeleteUnitTypeCommand() { Id = id }, cancellationToken);
            if (!res)
                return NotFound();

            return NoContent();
        }
    }

}
