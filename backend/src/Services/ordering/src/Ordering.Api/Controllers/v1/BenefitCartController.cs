using Ordering.Application.Features.BenefitCarts.Command;
using Ordering.Application.Features.BenefitCarts.Queries;

namespace Ordering.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class BenefitCartController : BaseApiController<BenefitCartController>
    {
        private readonly ILogger<BenefitCartController> _logger;
        public BenefitCartController(ILogger<BenefitCartController> logger)
        {
            _logger = logger;
        }
        // GET: api/<BenefitCartController>
        // <All BenefitCart> => For Admin
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get BenefitCart");

            var items = await Mediator.Send(new GetBenefitCartsQuery(), cancellationToken);
            var res = new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Get Data",
                Data = items
            };
            return Ok(res);
        }

        // GET api/<BenefitCartController>/5
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get BenefitCart by Id");
            var item = await Mediator.Send(new GetBenefitCartByIdQuery() { Id = id }, cancellationToken);
            if (item == null)
                return NotFound();

            return Ok(new BaseWithDataResponse { Success = true, Message = "Success Get Data", Data = item });
        }

        // POST api/<BenefitCartController>
        [HttpPost]
        public async Task<IActionResult> Post(AddBenefitCartCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.UserId))
            {
                return BadRequest(new BaseResponse { Success = false, Message = "UserId must be filled" });
            }

            return Ok(await Mediator.Send(command, cancellationToken));
        }

        // PUT api/<BenefitCartController>/5
        [HttpPut("{id}")]
        public async ValueTask<IActionResult> Put(string id, [FromBody] EditBenefitCartCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
            {
                return BadRequest(new BaseResponse { Success = false, Message = "Id must equal with body" });
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

        // DELETE api/<BenefitCartController>/5
        [HttpDelete("{id}")]
        public async ValueTask<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new BaseResponse { Success = false, Message = "Id must be filled" });
            }
            var res = await Mediator.Send(new DeleteBenefitCartCommand() { Id = id }, cancellationToken);
            if (!res)
                return NotFound();

            return NoContent();
        }
    }

}
