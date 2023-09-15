using Microsoft.AspNetCore.Authorization;

namespace Catalog.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ProductController : BaseApiController<ProductController>
    {
        private readonly ILogger<ProductController> _logger;
        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }
        // GET: api/<ProductController>
        // <All Product> => For Admin
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            var items = await Mediator.Send(new GetProductsQuery(), cancellationToken);
            var res = new BaseWithDataCountResponse
            {
                Success = true,
                Message = "Success Get Data",
                Count = items == null ? 0 : items.Count(),
                Data = items
            };
            return Ok(res);
        }
        // GET api/<ProductController>/5
        // <Get Product> => For Admin
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Product by Id");
            var items = await Mediator.Send(new GetProductByIdQuery() { Id = id }, cancellationToken);
            if (items == null)
            {
                return NotFound();
            }
            var res = new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Get Data",
                Data = items,
            };
            return Ok(res);
        }
        [HttpGet("[action]")]
        public async ValueTask<ActionResult> Paging([FromQuery] GetPagingProductsQuery request, CancellationToken cancellationToken)
        {
            var items = await Mediator.Send(request, cancellationToken);
            return Ok(items);
        }
        // <All Product By Category Slug> => For User Anonymous
        [HttpGet("[action]/{slug}")]
        public async ValueTask<ActionResult> ByCategorySlug(string slug, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Product by category slug");
            var items = await Mediator.Send(new GetProductsByCategorySlugQuery() { Slug = slug }, cancellationToken);
            //if (items == null)
            //{
            //    return NotFound();
            //}
            //var res = new ProductsByCategoryResponse
            //{
            //    Success = true,
            //    Message = "Success Get Data",
            //    Data = items
            //};

            return Ok(items);
        }

        // <All Adsense By User> => For User specific
        [HttpGet("[action]/{userid}")]
        public async ValueTask<ActionResult> ByUser(string userid, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Product by category slug");
            var items = await Mediator.Send(new GetProductsByUserQuery() { UserId = userid }, cancellationToken);
            if (items == null)
            {
                return NotFound();
            }
            return Ok(items);
        }
        // GET api/<ProductController>/5
        // <Get Product> => For User Anonymous
        [HttpGet("[action]/{slug}")]
        [AllowAnonymous]
        public async ValueTask<ActionResult> BySlug(string slug, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Product by slug");
            var items = await Mediator.Send(new GetProductBySlugQuery() { Slug = slug }, cancellationToken);
            if (items == null)
            {
                return NotFound();
            }
            var res = new BaseWithDataResponse<ProductItemResponse>
            {
                Success = true,
                Message = "Success Get Data",
                Data = items
            };
            return Ok(res);
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<IActionResult> Post(AddProductCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.UserId) ||
                string.IsNullOrEmpty(command.CategoryId) ||
                string.IsNullOrEmpty(command.Title))
            {
                return BadRequest(new ErrorResponse { Success = false, Message = "UserID, CategoryId, Title must be filled" });
            }

            return Ok(await Mediator.Send(command, cancellationToken));
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public async ValueTask<IActionResult> Put(string id, [FromBody] EditProductCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
            {
                return BadRequest(new ErrorResponse { Success = false, Message = "Id must equal with body" });
            }
            if (string.IsNullOrEmpty(command.UserId) ||
                string.IsNullOrEmpty(command.CategoryId) ||
                string.IsNullOrEmpty(command.Title))
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

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async ValueTask<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            if (id.IsNullOrEmpty())
            {
                return BadRequest(new ErrorResponse { Success = false, Message = "Id must be filled" });
            }
            var res = await Mediator.Send(new DeleteProductCommand() { Id = id }, cancellationToken);
            if (!res)
                return NotFound();

            return NoContent();
        }
    }
}
