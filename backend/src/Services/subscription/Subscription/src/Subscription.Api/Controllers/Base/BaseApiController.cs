using Mediator;

namespace Subscription.Api.Controllers.Base
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/v1/[controller]")]
    public abstract class BaseApiController<T> : ControllerBase
    {
        private IMediator? _mediatorInstance;
        private ILogger<T>? _loggerInstance;
        protected IMediator Mediator => _mediatorInstance ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
        protected ILogger<T>? Logger => _loggerInstance ??= HttpContext.RequestServices.GetRequiredService<ILogger<T>>();
    }
}
