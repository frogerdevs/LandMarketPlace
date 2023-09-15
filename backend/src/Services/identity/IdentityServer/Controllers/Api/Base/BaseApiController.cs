using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers.Api.Base
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseApiController<T> : ControllerBase
    {
        private IMediator? _mediatorInstance;
        private ILogger<T>? _loggerInstance;
        protected IMediator Mediator => _mediatorInstance ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
        protected ILogger<T>? Logger => _loggerInstance ??= HttpContext.RequestServices.GetRequiredService<ILogger<T>>();
    }
}
