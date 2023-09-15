namespace Catalog.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class CategoryController : BaseApiController<CategoryController>
    {
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(ILogger<CategoryController> logger)
        {
            _logger = logger;
        }
        // GET: api/<CategoryController>
        // <All Category> => For Admin
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Category");

            var items = await Mediator.Send(new GetCategoriesQuery(), cancellationToken);
            var res = new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Get Data",
                Data = items
            };
            return Ok(res);
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Category by Id");
            var items = await Mediator.Send(new GetCategoryByIdQuery() { Id = id }, cancellationToken);
            if (items == null)
            {
                return NotFound();
            }
            return Ok(items);
        }
        [HttpGet("[action]/{slug}")]
        public async ValueTask<ActionResult> BySlug(string slug, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Category by slug");
            var items = await Mediator.Send(new GetCategoryBySlugQuery() { Slug = slug }, cancellationToken);
            if (items == null)
            {
                return NotFound(new BaseResponse { Success = false, Message = "Not Found" });
            }
            var res = new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Get Data",
                Data = items
            };
            return Ok(res);
        }

        // POST api/<CategoryController>
        [HttpPost]
        public async Task<IActionResult> Post(AddCategoryCommand command, CancellationToken cancellationToken)
        {
            if (command.Name.IsNullOrEmpty())
            {
                return BadRequest(new ErrorResponse { Success = false, Message = "Name must be filled" });
            }

            return Ok(await Mediator.Send(command, cancellationToken));
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public async ValueTask<IActionResult> Put(string id, [FromBody] EditCategoryCommand command, CancellationToken cancellationToken)
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

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public async ValueTask<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            if (id.IsNullOrEmpty())
            {
                return BadRequest(new ErrorResponse { Success = false, Message = "Id must be filled" });
            }
            var res = await Mediator.Send(new DeleteCategoryCommand() { Id = id }, cancellationToken);
            if (!res)
                return NotFound();

            return NoContent();
        }
    }
}
