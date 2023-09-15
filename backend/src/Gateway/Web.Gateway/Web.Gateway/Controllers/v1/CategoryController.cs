using Microsoft.AspNetCore.Authorization;

namespace Web.Gateway.Controllers.v1
{
    [ApiVersion("1.0")]
    //[Authorize]
    public class CategoryController : BaseApiController<AboutController>
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;
        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService)
        {
            _logger = logger;

            _categoryService = categoryService;

        }
        // GET: api/<CategoryController>
        // <All Category> => For Admin
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Category");
            //var httpResponse = await _categoryService.GetAsync(cancellationToken);
            //return await httpResponse.ToActionResultAsync(cancellationToken);

            var response = await _categoryService.GetGrpcAsync(cancellationToken);
            return Ok(response);
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Id must be filled");
            _logger.LogInformation("Get Category by Id");
            var response = await _categoryService.GetGrpcByIdAsync(id, cancellationToken);
            if (response == null)
                return NotFound(new BaseResponse { Success = false, Message = "Data Tidak Ditemukan" });

            var result = new CategoryResponse
            {
                Success = true,
                Message = "Success Get Data",
                Data = response
            };
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("[action]/{slug}")]
        public async ValueTask<ActionResult> BySlug(string slug, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Category by Slug");
            var response = await _categoryService.GetBySlugAsync(slug, cancellationToken);
            if (response == null)
                return NotFound(new BaseResponse { Success = false, Message = "Daa Tidak Ditemukan" });
            var result = new CategoryResponse
            {
                Success = true,
                Message = "Success Get Data",
                Data = response
            };
            return Ok(result);
        }
        // POST api/<CategoryController>
        [HttpPost]
        public async Task<IActionResult> Post(CategoryRequest request, CancellationToken cancellationToken)
        {
            var httpResponse = await _categoryService.PostAsync(request, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public async ValueTask<IActionResult> Put(string id, [FromBody] CategoryPutRequest request, CancellationToken cancellationToken)
        {
            var httpResponse = await _categoryService.PutAsync(id, request, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public async ValueTask<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var httpResponse = await _categoryService.DeleteAsync(id, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
    }
}
