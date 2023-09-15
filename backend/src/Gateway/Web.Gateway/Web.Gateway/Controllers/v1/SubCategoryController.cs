using Microsoft.AspNetCore.Authorization;

namespace Web.Gateway.Controllers.v1
{
    [ApiVersion("1.0")]
    public class SubCategoryController : BaseApiController<SubCategoryController>
    {
        private readonly ILogger<SubCategoryController> _logger;
        private readonly ISubCategoryService _subCategoryService;
        public SubCategoryController(ILogger<SubCategoryController> logger, ISubCategoryService subcategoryService)
        {
            _logger = logger;
            _subCategoryService = subcategoryService;
        }
        // GET: api/<SubCategoryController>
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Category");
            var httpResponse = await _subCategoryService.GetAsync(cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // GET api/<SubCategoryController>/5
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Category by Id");
            var httpResponse = await _subCategoryService.GetByIdAsync(id, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
        [AllowAnonymous]
        [HttpGet("[action]/{slug}")]
        public async ValueTask<ActionResult> BySlug(string slug, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Category by Id");
            var httpResponse = await _subCategoryService.GetBySlugAsync(slug, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
        // POST api/<SubCategoryController>
        [HttpPost]
        public async Task<IActionResult> Post(SubCategoryRequest request, CancellationToken cancellationToken)
        {
            var httpResponse = await _subCategoryService.PostAsync(request, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // PUT api/<SubCategoryController>/5
        [HttpPut("{id}")]
        public async ValueTask<IActionResult> Put(string id, [FromBody] SubCategoryPutRequest request, CancellationToken cancellationToken)
        {
            var httpResponse = await _subCategoryService.PutAsync(id, request, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // DELETE api/<SubCategoryController>/5
        [HttpDelete("{id}")]
        public async ValueTask<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var httpResponse = await _subCategoryService.DeleteAsync(id, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
    }
}
