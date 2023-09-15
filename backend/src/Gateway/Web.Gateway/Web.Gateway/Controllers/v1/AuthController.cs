using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Web.Gateway.Controllers.v1
{
    [ApiVersion("1.0")]
    public class AuthController : BaseApiController<AuthController>
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        public AuthController(IUserService userService, IAuthService authService,
            ILogger<AuthController> logger)
        {
            _userService = userService;
            _authService = authService;
            _logger = logger;
        }
        [HttpGet("[action]/{emailorphone}")]
        public async ValueTask<ActionResult> CheckUser(string emailorphone, CancellationToken cancellationToken)
        {
            _logger.LogInformation("CheckUser");
            var response = await _userService.IsRegisteredAsync(emailorphone, cancellationToken);
            return await response.ToActionResultAsync(cancellationToken);
        }

        [HttpPost("[action]")]
        public async ValueTask<ActionResult> Token([FromForm] AuthTokenRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Token");
            var response = await _authService.PostToGetToken(request, cancellationToken);
            return await response.ToActionResultAsync(cancellationToken);
        }

        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        [HttpGet("[action]")]
        public async ValueTask<ActionResult> CsrfToken(CancellationToken cancellationToken)
        {
            var response = await _authService.GetCsrf(cancellationToken);
            return await response.ToActionResultAsync(cancellationToken);
        }

        [HttpPost("[action]")]
        //[ValidateAntiForgeryToken]
        public async ValueTask<ActionResult> Signout(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Signout");
            Dictionary<string, string?> formData = new();
            foreach (var keyValuePair in Request.Form)
            {
                formData.Add(keyValuePair.Key, keyValuePair.Value);
            }
            var response = await _authService.Signout(formData, cancellationToken);
            return await response.ToActionResultAsync(cancellationToken);
        }

        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        public async ValueTask<ActionResult> UserInfo(CancellationToken cancellationToken)
        {
            var response = await _authService.GetUserInfo(cancellationToken);
            return await response.ToActionResultAsync(cancellationToken);
        }
        [HttpPost("[action]")]
        public async ValueTask<ActionResult> RegisterMerchant([FromBody] RegisterMerchantRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("RegisterMerchant");

            var response = await _authService.RegisterMerchant(request, cancellationToken);
            return await response.ToActionResultAsync(cancellationToken);
        }
        [HttpPost("[action]")]
        public async ValueTask<ActionResult> RegisterLanders([FromBody] RegisterLandersRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("RegisterLanders");

            var response = await _authService.RegisterLanders(request, cancellationToken);
            return await response.ToActionResultAsync(cancellationToken);
        }
        [HttpPost("[action]")]
        public async ValueTask<ActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ForgotPassword");

            var response = await _authService.ForgotPassword(request, cancellationToken);
            return await response.ToActionResultAsync(cancellationToken);
        }
        [HttpPost("[action]")]
        public async ValueTask<ActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ResetPassword");

            var response = await _authService.ResetPassword(request, cancellationToken);
            return await response.ToActionResultAsync(cancellationToken);
        }
    }
}
