using Microsoft.AspNetCore.Mvc;

namespace Web.Gateway.Controllers.v1
{
    [ApiVersion("1.0")]
    public class OtpController : BaseApiController<OtpController>
    {
        private readonly ILogger<OtpController> _logger;
        private readonly IOtpService _otpService;
        public OtpController(IOtpService otpService, ILogger<OtpController> logger)
        {
            _otpService = otpService;
            _logger = logger;
        }
        [HttpPost("[action]")]
        public async ValueTask<ActionResult> SendOtp([FromBody] OtpRequest request, CancellationToken cancellationToken = default)
        {
            // Validate request
            if (string.IsNullOrEmpty(request.EmailOrPhone))
            {
                return BadRequest(new BaseResponse { Success = false, Message = "Invalid email" });
            }

            var httpResponse = await _otpService.SendOtpAsync(request, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
        [HttpPost("[action]")]
        public async ValueTask<ActionResult> VerifyOtp([FromBody] VerifyOtpRequest request, CancellationToken cancellationToken = default)
        {
            // Validate request
            if (string.IsNullOrEmpty(request.EmailOrPhone))
            {
                return BadRequest(new BaseResponse { Success = false, Message = "Invalid email" });
            }

            var httpResponse = await _otpService.VerifyOtpAsync(request, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);

        }
    }
}
