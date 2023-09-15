using IdentityServer.Features.About.Commands;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers.Api.v1
{
    [ApiVersion("1.0")]
    public class AboutController : BaseApiController<AboutController>
    {
        private readonly ILogger<AboutController> _logger;
        public AboutController(ILogger<AboutController> logger)
        {
            _logger = logger;
        }
        [HttpGet("{email}")]
        public async ValueTask<ActionResult> Get(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email) || !ValidateInput.IsValidEmail(email))
            {
                return BadRequest();
            }
            var user = await Mediator.Send(new GetAboutByEmailQuery() { Email = email }, cancellationToken);
            if (user == null)
            {
                return NotFound();
            }
            var res = new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Get Data",
                Data = user
            };
            return Ok(res);
        }
        [HttpPut("{email}")]
        public async ValueTask<ActionResult> Put(string email, EditAboutCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email) || !ValidateInput.IsValidEmail(request.Email))
            {
                return BadRequest(new BaseResponse { Success = false, Message = "Email tidak valid" });
            }
            if (email != request.Email)
            {
                return BadRequest(new BaseResponse { Success = false, Message = "Id must equal with body" });
            }

            var user = await Mediator.Send(request, cancellationToken);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
