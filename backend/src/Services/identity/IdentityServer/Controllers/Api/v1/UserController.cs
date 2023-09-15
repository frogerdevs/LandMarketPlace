using IdentityServer.Controllers.Api.Base;
using IdentityServer.Features.Users.Commands;
using IdentityServer.Features.Users.Queries;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IdentityServer.Controllers.Api.v1
{
    [ApiVersion("1.0")]
    public class UserController : BaseApiController<UserController>
    {
        private readonly ILogger<UserController> _logger;
        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }
        // GET: api/<UserController>
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            var users = await Mediator.Send(new GetUsersQuery(), cancellationToken);
            return Ok(users);
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            var user = await Mediator.Send(new GetUserByIdQuery() { Id = id }, cancellationToken);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet("[action]/{emailorphone}")]
        public async ValueTask<ActionResult> ByEmail(string emailorphone, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(emailorphone))
            {
                return BadRequest();
            }
            var user = await Mediator.Send(new GetUserByEmailOrPhoneQuery() { EmailOrPhone = emailorphone }, cancellationToken);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet("[action]/{emailorphone}")]
        public async ValueTask<ActionResult> IsRegistered(string emailorphone, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(emailorphone))
            {
                return BadRequest();
            }
            var IsRegisteredUser = await Mediator.Send(new IsRegisteredUserQuery() { EmailOrPhone = emailorphone }, cancellationToken);

            return Ok(IsRegisteredUser);
        }
        // POST api/<UserController>
        [HttpPost]
        public async Task<IActionResult> Post(AddUserCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.Email)
                || string.IsNullOrEmpty(command.Password)
                || string.IsNullOrEmpty(command.PhoneNumber))
            {
                return BadRequest(new { Success = false, Message = "Semua field mandatory harus di isi" });
            }
            var user = await Mediator.Send(command, cancellationToken);
            if (user == null || !user.Success)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, (user == null) ? new { Success = false, Message = "Gagal menambahkan data" } : new { Success = false, Message = user.Message });
            }
            return Ok(user);
        }


        // PUT api/<UserController>/5
        [HttpPut("{email}")]
        public async Task<IActionResult> Put(string email, [FromBody] EditUserCommand command, CancellationToken cancellationToken)
        {
            if (email != command.Email)
            {
                return BadRequest(new { Success = false, Message = "Email harus sesuai dengan body" });
            }

            if (string.IsNullOrEmpty(command.Email)
               || string.IsNullOrEmpty(command.Password)
               || string.IsNullOrEmpty(command.PhoneNumber))
            {
                return BadRequest(new { Success = false, Message = "Semua field mandatory harus di isi" });
            }
            try
            {
                var user = await Mediator.Send(command, cancellationToken);

                if (user is null)
                    return NotFound();
                if (!user.Success)
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, user.Message);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Failed update data {Message}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed update data");
            }

        }

        // DELETE api/<UserController>/5
        [HttpDelete("{email}")]
        public async Task<IActionResult> Delete(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { Success = false, Message = "Email harus di isi." });
            }
            var res = await Mediator.Send(new DeleteUserCommand() { Email = email }, cancellationToken);
            if (res is null)
                return NotFound();
            if (!res.Success)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, res.Message);
            }

            return NoContent();
        }
    }
}
