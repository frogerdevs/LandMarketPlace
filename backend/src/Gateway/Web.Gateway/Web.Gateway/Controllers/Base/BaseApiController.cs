using Microsoft.AspNetCore.Mvc;

namespace Web.Gateway.Controllers.Base
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseApiController<T> : ControllerBase
    {
        private ILogger<T>? _loggerInstance;
        protected ILogger<T> Logger => _loggerInstance ??= HttpContext.RequestServices.GetRequiredService<ILogger<T>>();
    }
}
