using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Web.Gateway.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ProductController : BaseApiController<ProductController>
    {
        readonly IProductService _productsService;
        public ProductController(IProductService productsService)
        {
            _productsService = productsService;
        }
        // <All Product> => For Admin
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Get Product");
            var response = await _productsService.GetAsync(cancellationToken);
            var result = new BaseListResponse<ProductsItem?>()
            {
                Success = true,
                Message = "Success Get Data",
                Data = response
            };
            return Ok(result);
        }
        // <All Product> => For Admin
        [HttpGet("[action]")]
        public async ValueTask<ActionResult> Paging([FromQuery] PagingProductRequest request, CancellationToken cancellationToken)
        {
            Logger.LogInformation("Get Product");
            //var queryString = QueryHelpers.ParseQuery(request);
            var response = await _productsService.GetPagingAsync(request, cancellationToken);
            return Ok(response);
        }

        // <All Product By Category Slug> => For User Anonymous
        [HttpGet("[action]")]
        public async ValueTask<ActionResult> ByCategorySlug([FromQuery] PagingBySlugRequest request, CancellationToken cancellationToken)
        {
            Logger.LogInformation("Get Product by category slug");
            var response = await _productsService.GetByCategorySlugAsync(request, cancellationToken);
            return Ok(response);
        }

        // GET api/<ProductController>/5
        // <Get Product> => For Admin, merchant
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            Logger.LogInformation("Get Product by Id");
            var response = await _productsService.GetByIdAsync(id, cancellationToken);
            if (response == null)
                return NotFound(new BaseResponse { Success = false, Message = "Data Tidak di temukan" });
            var result = new BaseWithDataResponse<ProductItemResponse?>()
            {
                Success = true,
                Message = "Success Get Data",
                Data = response
            };
            return Ok(result);
        }

        // GET api/<ProductController>/5
        // <Get Product> => For User Anonymous
        [HttpGet("[action]/{slug}")]
        [AllowAnonymous]
        public async ValueTask<ActionResult> BySlug(string slug, CancellationToken cancellationToken)
        {
            Logger.LogInformation("Get Product by slug");
            var response = await _productsService.GetBySlugAsync(slug, cancellationToken);
            if (response == null)
                return NotFound(new BaseResponse { Success = false, Message = "Data Tidak di temukan" });
            var result = new BaseWithDataResponse<ProductItemResponse?>()
            {
                Success = true,
                Message = "Success Get Data",
                Data = response
            };
            return Ok(result);
        }
        [HttpGet("[action]/{userid}")]
        public async ValueTask<ActionResult> ByUser(string userid, CancellationToken cancellationToken)
        {
            Logger.LogInformation("Get Products by User");
            var httpResponse = await _productsService.GetByUserAsync(userid, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<IActionResult> Post(ProductRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.UserId) ||
                string.IsNullOrEmpty(request.CategoryId) ||
                string.IsNullOrEmpty(request.Title))
            {
                return BadRequest(new BaseResponse { Success = false, Message = "UserID, CategoryId, Title must be filled" });
            }

            var httpResponse = await _productsService.PostAsync(request, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public async ValueTask<IActionResult> Put(string id, [FromBody] ProductPutRequest request, CancellationToken cancellationToken)
        {
            if (id != request.Id)
            {
                return BadRequest(new BaseResponse { Success = false, Message = "Id must equal with body" });
            }
            if (string.IsNullOrEmpty(request.UserId) ||
                string.IsNullOrEmpty(request.CategoryId) ||
                string.IsNullOrEmpty(request.Title))
            {
                return BadRequest(new BaseResponse { Success = false, Message = "userID, CategoryId, Title must be filled" });
            }
            try
            {
                var httpResponse = await _productsService.PutAsync(id, request, cancellationToken);
                return await httpResponse.ToActionResultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogInformation("Failed update data {Message}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed update data");
            }
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async ValueTask<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            if (id.IsNullOrEmpty())
            {
                return BadRequest(new BaseResponse { Success = false, Message = "Id must be filled" });
            }
            var httpResponse = await _productsService.DeleteAsync(id, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
    }
}
