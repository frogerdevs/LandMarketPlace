using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers.Web.Base
{
    public class BaseWebController<T> : Controller
    {
        private ILogger<T>? logger;
        protected ILogger<T>? Logger => logger ??= HttpContext.RequestServices.GetService<ILogger<T>?>();
    }
}
