namespace Catalog.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class SubCategoryController : BaseApiController<CategoryController>
    {
        private readonly ILogger<SubCategoryController> _logger;
        public SubCategoryController(ILogger<SubCategoryController> logger)
        {
            _logger = logger;
        }
        // GET: api/<SubCategoryController>
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Category");
            var items = await Mediator.Send(new GetSubCategoriesQuery(), cancellationToken);
            return Ok(items);
        }

        // GET api/<SubCategoryController>/5
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Category by Id");
            var items = await Mediator.Send(new GetSubCategoryByIdQuery() { Id = id }, cancellationToken);
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
            var items = await Mediator.Send(new GetSubCategoryBySlugQuery() { Slug = slug }, cancellationToken);
            if (items == null)
            {
                return NotFound();
            }
            return Ok(items);
        }
        [HttpGet("[action]/{categoryid}")]
        public async ValueTask<ActionResult> ByCategory(string categoryid, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Category by slug");
            var items = await Mediator.Send(new GetSubCategoryByCategoryIdQuery() { CategoryId = categoryid }, cancellationToken);
            if (items == null)
            {
                return NotFound();
            }
            return Ok(items);
        }
        // POST api/<SubCategoryController>
        [HttpPost]
        public async Task<IActionResult> Post(AddSubCategoryCommand command, CancellationToken cancellationToken)
        {
            if (command.Name.IsNullOrEmpty())
            {
                return BadRequest(new ErrorResponse { Success = false, Message = "Name must be filled" });
            }

            return Ok(await Mediator.Send(command, cancellationToken));
        }

        // PUT api/<SubCategoryController>/5
        [HttpPut("{id}")]
        public async ValueTask<IActionResult> Put(string id, [FromBody] EditSubCategoryCommand command, CancellationToken cancellationToken)
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

        // DELETE api/<SubCategoryController>/5
        [HttpDelete("{id}")]
        public async ValueTask<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            if (id.IsNullOrEmpty())
            {
                return BadRequest(new ErrorResponse { Success = false, Message = "Id must be filled" });
            }
            var res = await Mediator.Send(new DeleteSubCategoryCommand() { Id = id }, cancellationToken);
            if (!res)
                return NotFound();

            return NoContent();
        }
    }
}
