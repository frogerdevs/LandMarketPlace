using Catalog.Application.Features.Adsenses.Commands;
using Catalog.Application.Features.Adsenses.Queries;

namespace Catalog.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class AdsenseController : BaseApiController<AdsenseController>
    {
        private readonly ILogger<AdsenseController> _logger;
        public AdsenseController(ILogger<AdsenseController> logger)
        {
            _logger = logger;
        }
        // GET: api/<AdsenseController>
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            var items = await Mediator.Send(new GetAdsensesQuery(), cancellationToken);
            _logger.LogInformation("Get Adsense : {items}", items);
            return Ok(items);
        }

        // GET api/<AdsenseController>/5
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            var items = await Mediator.Send(new GetAdsenseByIdQuery() { Id = id }, cancellationToken);
            _logger.LogInformation("Get Adsense by Id : {items}", items);
            if (items == null)
            {
                return NotFound();
            }
            return Ok(items);
        }
        [HttpGet("[action]/{slug}")]
        public async ValueTask<ActionResult> BySlug(string slug, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Adsense by slug");
            var items = await Mediator.Send(new GetAdsenseBySlugQuery() { Slug = slug }, cancellationToken);
            if (items == null)
            {
                return NotFound();
            }
            return Ok(items);
        }
        // <All Adsense By User> => For User specific
        [HttpGet("[action]/{userid}")]
        public async ValueTask<ActionResult> ByUser(string userid, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Product by category slug");
            var items = await Mediator.Send(new GetAdsenseByUserQuery() { UserId = userid }, cancellationToken);
            if (items == null)
            {
                return NotFound();
            }
            return Ok(items);
        }
        // POST api/<AdsenseController>
        [HttpPost]
        public async Task<IActionResult> Post(AddAdsenseCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.ProductId))
            {
                return BadRequest(new ErrorResponse { Success = false, Message = "ProductId must be filled" });
            }

            return Ok(await Mediator.Send(command, cancellationToken));
        }

        // PUT api/<AdsenseController>/5
        [HttpPut("{id}")]
        public async ValueTask<IActionResult> Put(string id, [FromBody] EditAdsenseCommand command, CancellationToken cancellationToken)
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

        // DELETE api/<AdsenseController>/5
        [HttpDelete("{id}")]
        public async ValueTask<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            if (id.IsNullOrEmpty())
            {
                return BadRequest(new ErrorResponse { Success = false, Message = "Id must be filled" });
            }
            var res = await Mediator.Send(new DeleteAdsenseCommand() { Id = id }, cancellationToken);
            if (!res)
                return NotFound();

            return NoContent();
        }
    }

}
