namespace Web.Gateway.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        readonly ICategoryService _categoryService;
        readonly IInDealsService _dealService;
        public HomeController(ILogger<HomeController> logger,
            ICategoryService categoryService, IInDealsService dealService)
        {
            _logger = logger;
            _categoryService = categoryService;
            _dealService = dealService;
        }

        // GET: api/Category
        [HttpGet("[action]")]
        public async Task<ActionResult> Category()
        {
            _logger.LogInformation("Get Category");
            var items = await _categoryService.GetForHomePageAsync();
            return Ok(items);
        }

        // GET: api/Deals
        [HttpGet("[action]")]
        public async Task<ActionResult> Deals()
        {
            _logger.LogInformation("Get Deals");
            var items = await _dealService.GetForHomePageAsync();
            return Ok(items);
        }

    }
}
