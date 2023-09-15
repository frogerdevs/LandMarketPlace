using Microsoft.AspNetCore.Authorization;

namespace Web.Gateway.Controllers.v1
{
    [ApiVersion("1.0")]
    public class AboutController : BaseApiController<AboutController>
    {
        private readonly ILogger<AboutController> _logger;
        private readonly IAboutService _aboutService;
        public AboutController(ILogger<AboutController> logger, IAboutService aboutService)
        {
            _logger = logger;
            _aboutService = aboutService;

        }
        // GET: api/<AboutController>
        [AllowAnonymous]
        [HttpGet("{email}")]
        public async ValueTask<ActionResult> Get(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest();
            }
            var response = await _aboutService.GetAboutAsync(email, cancellationToken);

            if (response == null)
                return NotFound(new BaseResponse { Success = false, Message = "Data Tidak Ditemukan" });

            var result = new BaseWithDataResponse<AboutResponse>
            {
                Success = true,
                Message = "Success Get Data",
                Data = response
            };
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("[action]/{userid}")]
        public async ValueTask<ActionResult> ByUser(string userid, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(userid))
            {
                return BadRequest();
            }
            var response = await _aboutService.GetAboutByUserAsync(userid, cancellationToken);

            if (response == null)
                return NotFound(new BaseResponse { Success = false, Message = "Data Tidak Ditemukan" });

            var result = new BaseWithDataResponse<AboutResponse>
            {
                Success = true,
                Message = "Success Get Data",
                Data = response
            };
            return Ok(result);
        }
        // PUT api/<AboutController>/5
        [HttpPut("{email}")]
        public async ValueTask<ActionResult> Put(string email, [FromBody] EditAboutRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest(new BaseResponse { Success = false, Message = "Email tidak valid" });
            }
            if (email != request.Email)
            {
                return BadRequest(new BaseResponse { Success = false, Message = "Id must equal with body" });
            }
            var httpResponse = await _aboutService.PutAboutAsync(email, request, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

    }
}
