using Microsoft.AspNetCore.Mvc;
using Web.Gateway.Controllers.Base;
using Web.Gateway.Dto.Request.Users;
using Web.Gateway.Extension;
using Web.Gateway.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Gateway.Controllers.v1
{
    [ApiVersion("1.0")]
    public class UserController : BaseApiController<UserController>
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        // GET: api/<UserController>
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            var httpResponse = await _userService.GetUsersAsync(cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            var httpResponse = await _userService.GetByIdAsync(id, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
        [HttpGet("[action]/{emailorphone}")]
        public async ValueTask<ActionResult> ByEmail(string emailorphone, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(emailorphone))
            {
                return BadRequest();
            }
            var httpResponse = await _userService.GetByEmailAsync(emailorphone, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
        [HttpGet("[action]/{emailorphone}")]
        public async ValueTask<ActionResult> IsRegistered(string emailorphone, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(emailorphone))
            {
                return BadRequest();
            }
            var httpResponse = await _userService.IsRegisteredAsync(emailorphone, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // POST api/<UserController>
        [HttpPost()]
        public async ValueTask<ActionResult> Post([FromBody] UserItemRequest request, CancellationToken cancellationToken)
        {
            if (request == null) return BadRequest("Request null");

            if (request.Password == null || request.Email == null || request.PhoneNumber == null) return BadRequest("Nama, Email, & Nomor Telpon harus di isi.");

            var httpResponse = await _userService.PostAsync(request, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // PUT api/<UserController>/5
        [HttpPut("{email}")]
        public async Task<IActionResult> Put(string email, [FromBody] UserItemRequest request, CancellationToken cancellationToken)
        {
            if (email != request.Email)
            {
                return BadRequest(new { Success = false, Message = "Email harus sesuai dengan body" });
            }

            if (string.IsNullOrEmpty(request.Email)
               || string.IsNullOrEmpty(request.Password)
               || string.IsNullOrEmpty(request.PhoneNumber))
            {
                return BadRequest(new { Success = false, Message = "Semua field mandatory harus di isi" });
            }
            var httpResponse = await _userService.PutAsync(email, request, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
        // DELETE api/<UserController>/5
        [HttpDelete("{email}")]
        public async Task<IActionResult> Delete(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { Success = false, Message = "Email harus di isi." });
            }
            var httpResponse = await _userService.DeleteAsync(email, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }


    }
}
