using Web.Gateway.Dto.Request.Profiles;
using Web.Gateway.Dto.Response.Profile;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Gateway.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ProfileController : BaseApiController<UserController>
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly IProfileService _profileService;
        public ProfileController(IProfileService userService, ILogger<ProfileController> logger)
        {
            _profileService = userService;
            _logger = logger;
        }
        // GET: api/<ProfileController>
        [HttpGet("[action]/{email}")]
        public async ValueTask<ActionResult> Landers(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest();
            }
            var httpResponse = await _profileService.GetLandersAsync(email, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        [HttpGet("[action]/{email}")]
        public async ValueTask<ActionResult> Merchant(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest();
            }
            var response = await _profileService.GetMerchantAsync(email, cancellationToken);
            if (response == null)
            {
                return NotFound();
            }
            if (!response.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed get data");
            }
            return Ok(response);
        }
        [HttpGet("[action]/{slug}")]
        public async ValueTask<ActionResult> ByBrandSlug(string slug, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return BadRequest();
            }
            var response = await _profileService.GetProfileBySlugAsync(slug, cancellationToken);
            if (response == null)
            {
                return NotFound();
            }
            var result = new BaseWithDataResponse<ProfileByBrandSlugResponse?>()
            {
                Success = true,
                Message = "Success Get Data",
                Data = response
            };
            return Ok(result);
        }

        [HttpPut("[action]/{email}")]
        public async ValueTask<ActionResult> Landers(string email, EditProfileLandersRequest command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(command.Email))
            {
                return BadRequest();
            }
            if (email != command.Email)
            {
                return BadRequest("Email Harus Sesuai");
            }
            var httpResponse = await _profileService.PutLandersAsync(command, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
        [HttpPut("[action]/{email}")]
        public async ValueTask<ActionResult> Merchant(string email, EditProfileMerchantRequest command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(command.Email))
            {
                return BadRequest();
            }
            if (email != command.Email)
            {
                return BadRequest("Email Harus Sesuai");
            }
            var httpResponse = await _profileService.PutMerchantAsync(command, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
        [HttpPost("[action]")]
        public async ValueTask<ActionResult> MerchantVerification(AddMerchantVerificationRequest command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.Email))
            {
                return BadRequest();
            }
            var httpResponse = await _profileService.AddMerchantVerificationAsync(command, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
        [HttpPost("[action]")]
        public async ValueTask<ActionResult> ChangePassword(ChangePasswordRequest command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.Email))
            {
                return BadRequest();
            }
            var httpResponse = await _profileService.ChangePasswordAsync(command, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
    }
}
