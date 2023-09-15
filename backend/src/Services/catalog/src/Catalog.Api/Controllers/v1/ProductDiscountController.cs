using Microsoft.AspNetCore.Authorization;

namespace Catalog.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ProductDiscountController : BaseApiController<ProductDiscountController>
    {
        private readonly ILogger<ProductDiscountController> _logger;
        public ProductDiscountController(ILogger<ProductDiscountController> logger)
        {
            _logger = logger;
        }
        // GET: api/<ProductDiscountController>
        // <All ProductDiscount> => For Admin
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            var items = await Mediator.Send(new GetProductDiscountsQuery(), cancellationToken);
            var res = new BaseWithDataCountResponse
            {
                Success = true,
                Message = "Success Get Data",
                Data = items
            }; return Ok(res);
        }
        // <All ProductDiscount By User> => For User specific
        [HttpGet("[action]/{userid}")]
        public async ValueTask<ActionResult> ByUser(string userid, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Product by category slug");
            var items = await Mediator.Send(new GetProductDiscountsByUserQuery() { UserId = userid }, cancellationToken);
            if (items == null)
            {
                return NotFound();
            }
            return Ok(items);
        }

        // GET api/<ProductDiscountController>/5
        // <Get ProductDiscount> => For Admin
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Product by Id");
            var items = await Mediator.Send(new GetProductDiscountByIdQuery() { Id = id }, cancellationToken);
            if (items == null)
            {
                return NotFound();
            }
            var res = new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Get Data",
                Data = items
            };

            return Ok(res);
        }

        // GET api/<ProductDiscountController>/5
        // <Get ProductDiscount> => For User Anonymous
        [HttpGet("[action]/{slug}")]
        [AllowAnonymous]
        public async ValueTask<ActionResult> BySlug(string slug, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Product by slug");
            var items = await Mediator.Send(new GetProductDiscountBySlugQuery() { Slug = slug }, cancellationToken);
            if (items == null)
            {
                return NotFound();
            }
            return Ok(items);
        }

        // POST api/<ProductDiscountController>
        [HttpPost]
        public async Task<IActionResult> Post(AddProductDiscountCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.UserId) ||
                string.IsNullOrEmpty(command.ProductId))
            {
                return BadRequest(new ErrorResponse { Success = false, Message = "UserID, CategoryId, Title must be filled" });
            }

            return Ok(await Mediator.Send(command, cancellationToken));
        }

        // PUT api/<ProductDiscountController>/5
        [HttpPut("{id}")]
        public async ValueTask<IActionResult> Put(string id, [FromBody] EditProductDiscountCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
            {
                return BadRequest(new ErrorResponse { Success = false, Message = "Id must equal with body" });
            }
            if (string.IsNullOrEmpty(command.UserId) ||
                string.IsNullOrEmpty(command.ProductId))
            {
                return BadRequest(new ErrorResponse { Success = false, Message = "userID, CategoryId, Title must be filled" });
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
        // PUT api/<ProductDiscountController>/5
        [HttpPut("[action]/{id}")]
        public async ValueTask<IActionResult> Activate(string id, [FromBody] ActivateProductDiscountCommand command, CancellationToken cancellationToken)
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

        // DELETE api/<ProductDiscountController>/5
        [HttpDelete("{id}")]
        public async ValueTask<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            if (id.IsNullOrEmpty())
            {
                return BadRequest(new ErrorResponse { Success = false, Message = "Id must be filled" });
            }
            var res = await Mediator.Send(new DeleteProductDiscountCommand() { Id = id }, cancellationToken);
            if (!res)
                return NotFound();

            return NoContent();
        }
    }

}
