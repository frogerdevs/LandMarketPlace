using IdentityServer.Features.Profile.Commands;
using IdentityServer.Features.Profile.Queries;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IdentityServer.Controllers.Api.v1
{
    [ApiVersion("1.0")]
    public class ProfileController : BaseApiController<UserController>
    {
        private readonly ILogger<ProfileController> _logger;
        public ProfileController(ILogger<ProfileController> logger)
        {
            _logger = logger;
        }

        // GET api/<ProfileController>/5
        [HttpGet("[action]/{email}")]
        public async ValueTask<ActionResult> Landers(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email) || !ValidateInput.IsValidEmail(email))
            {
                return BadRequest();
            }
            var user = await Mediator.Send(new GetProfileLandersByEmailQuery() { Email = email }, cancellationToken);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet("[action]/{email}")]
        public async ValueTask<ActionResult> Merchant(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest();
            }
            var user = await Mediator.Send(new GetProfileMerchantByEmailQuery() { Email = email }, cancellationToken);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // PUT api/<ProfileController>/5
        [HttpPut("[action]/{email}")]
        public async ValueTask<ActionResult> Landers(string email, EditProfileLandersCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(command.Email) || !ValidateInput.IsValidEmail(command.Email))
            {
                return BadRequest();
            }
            var user = await Mediator.Send(command, cancellationToken);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPut("[action]/{email}")]
        public async ValueTask<ActionResult> Merchant(string email, EditProfileMerchantCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(command.Email) || !ValidateInput.IsValidEmail(command.Email))
            {
                return BadRequest();
            }
            var user = await Mediator.Send(command, cancellationToken);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("[action]")]
        public async ValueTask<ActionResult> ChangePassword(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.Email) || !ValidateInput.IsValidEmail(command.Email))
            {
                return BadRequest();
            }
            var user = await Mediator.Send(command, cancellationToken);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

    }
}
