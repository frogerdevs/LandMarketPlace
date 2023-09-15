using Microsoft.AspNetCore.Authorization;

namespace Web.Gateway.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ProductDiscountController : BaseApiController<ProductDiscountController>
    {
        private readonly ILogger<ProductDiscountController> _logger;
        private readonly IProductDiscountService _productDiscountService;
        public ProductDiscountController(ILogger<ProductDiscountController> logger, IProductDiscountService productDiscountService)
        {
            _logger = logger;
            _productDiscountService = productDiscountService;
        }
        // GET: api/<ProductDiscountController>
        // <All ProductDiscount> => For Admin
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get ProductDiscount");
            var response = await _productDiscountService.GetAsync(cancellationToken);
            var result = new BaseListResponse<ProductDiscountItem>()
            {
                Success = true,
                Message = "Success Get Data",
                Data = response
            };
            return Ok(result);
        }

        // GET api/<ProductDiscountController>/5
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get ProductDiscount by Id");
            var response = await _productDiscountService.GetByIdAsync(id, cancellationToken);
            if (response == null)
            {
                return NotFound();
            }

            var result = new BaseWithDataResponse<ProductDiscountItem>()
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
            _logger.LogInformation("Get ProductDiscount by Slug");
            var httpResponse = await _productDiscountService.GetBySlugAsync(slug, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
        [HttpGet("[action]/{userid}")]
        public async ValueTask<ActionResult> ByUser(string userid, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get ProductDiscount by User");
            var httpResponse = await _productDiscountService.GetByUserAsync(userid, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
        [AllowAnonymous]
        [HttpGet("[action]/{categoryslug}")]
        public async ValueTask<ActionResult> ByCategorySlug(string categoryslug, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get ProductDiscount by Slug");
            var response = await _productDiscountService.GetByCategorySlugAsync(categoryslug, cancellationToken);
            var result = new BaseListResponse<ProductDiscountByCategorySlugItem>()
            {
                Success = true,
                Message = "Success Get Data",
                Data = response
            };
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("[action]")]
        public async ValueTask<ActionResult> OfTheWeek(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get ProductDiscount OfTheWeek");
            var response = await _productDiscountService.GetOfTheweekAsync(cancellationToken);
            var result = new BaseListResponse<ProductDiscountsOfTheWeekItem>()
            {
                Success = true,
                Message = "Success Get Data",
                Data = response
            };
            return Ok(result);
        }

        // POST api/<ProductDiscountController>
        [HttpPost]
        public async Task<IActionResult> Post(ProductDiscountRequest request, CancellationToken cancellationToken)
        {
            var httpResponse = await _productDiscountService.PostAsync(request, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // PUT api/<ProductDiscountController>/5
        [HttpPut("{id}")]
        public async ValueTask<IActionResult> Put(string id, [FromBody] ProductDiscountPutRequest request, CancellationToken cancellationToken)
        {
            var httpResponse = await _productDiscountService.PutAsync(id, request, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
        // PUT api/<ProductDiscountController>/5
        [HttpPut("[action]/{id}")]
        public async ValueTask<IActionResult> Activate(string id, [FromBody] ActivateProductDiscountRequest request, CancellationToken cancellationToken)
        {
            var httpResponse = await _productDiscountService.ActivateAsync(id, request, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
        // DELETE api/<ProductDiscountController>/5
        [HttpDelete("{id}")]
        public async ValueTask<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var httpResponse = await _productDiscountService.DeleteAsync(id, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
    }

}
