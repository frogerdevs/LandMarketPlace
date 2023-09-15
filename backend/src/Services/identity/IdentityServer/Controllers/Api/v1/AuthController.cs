using IdentityServer.Features.Users.Commands;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server.AspNetCore;
using OpenIddict.Validation.AspNetCore;

namespace IdentityServer.Controllers.Api.v1
{
    [ApiVersion("1.0")]
    public class AuthController : BaseApiController<AuthController>
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAntiforgery _antiforgery;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(IAntiforgery antiforgery, SignInManager<AppUser> signInManager, ILogger<AuthController> logger)
        {
            _signInManager = signInManager;
            _antiforgery = antiforgery;
            _logger = logger;
        }
        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        public IActionResult CsrfToken(CancellationToken cancellationToken)
        {
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            Response.Cookies.Append("X-CSRF-TOKEN-COOKIE", tokens.RequestToken!, new CookieOptions { HttpOnly = false });

            return Ok(new { csrfToken = tokens.RequestToken });
        }
        [HttpPost("[action]")]
        //[ValidateAntiForgeryToken]
        public async ValueTask<ActionResult> SignOut(CancellationToken cancellationToken)
        {
            // Ask ASP.NET Core Identity to delete the local and external cookies created
            // when the user agent is redirected from the external identity provider
            // after a successful authentication flow (e.g Google or Facebook).
            await _signInManager.SignOutAsync();

            // Returning a SignOutResult will ask OpenIddict to redirect the user agent
            // to the post_logout_redirect_uri specified by the client application or to
            // the RedirectUri specified in the authentication properties if none was set.
            SignOut(authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            return Ok(new { message = "Sign out successful." });
        }
        [HttpPost("[action]")]
        public async ValueTask<ActionResult> RegisterMerchant([FromBody] RegisterMerchantCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("RegisterMerchant");
            if (string.IsNullOrEmpty(command.Email)
                || string.IsNullOrEmpty(command.Password)
                || string.IsNullOrEmpty(command.PhoneNumber))
            {
                return BadRequest(new { Success = false, Message = "Semua field mandatory harus di isi" });
            }
            var user = await Mediator.Send(command, cancellationToken);
            if (user == null || !user.Success)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, (user == null) ? new { Success = false, Message = "Gagal Register" } : new { Success = false, Message = user.Message });
            }
            return Ok(user);
        }
        [HttpPost("[action]")]
        public async ValueTask<ActionResult> RegisterLanders([FromBody] RegisterLandersCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("RegisterMerchant");
            if (string.IsNullOrEmpty(command.Email))
            {
                return BadRequest(new { Success = false, Message = "Semua field mandatory harus di isi" });
            }
            var user = await Mediator.Send(command, cancellationToken);
            if (user == null || !user.Success)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, (user == null) ? new { Success = false, Message = "Gagal Register" } : new { Success = false, Message = user.Message });
            }
            return Ok(user);
        }
        [HttpPost("[action]")]
        public async ValueTask<ActionResult> ForgotPassword([FromBody] ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ForgotPassword");
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest(new { Success = false, Message = "Email harus di isi" });
            }
            try
            {
                var res = await Mediator.Send(request, cancellationToken);
                if (res == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        Success = false,
                        Message = "Email tidak ditemukan."
                    });
                }
                if (!res.Success)
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, new { Success = false, res.Message });
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Success = false, Message = ex.Message });
            }
        }
        [HttpPost("[action]")]
        public async ValueTask<ActionResult> ResetPassword([FromBody] ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ResetPassword");
            if (string.IsNullOrEmpty(request.Code) || string.IsNullOrEmpty(request.Email))
            {
                return BadRequest(new { Success = false, Message = "Harus menyertakan Code & Email reset password" });
            }
            try
            {
                var res = await Mediator.Send(request, cancellationToken);
                if (res == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        Success = false,
                        Message = "Email tidak ditemukan."
                    });
                }
                if (!res.Success)
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, new { Success = false, res.Message });
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Success = false, Message = ex.Message });
            }
        }
    }
}
