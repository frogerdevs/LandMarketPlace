using Microsoft.AspNetCore.Authorization;

namespace Catalog.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [AllowAnonymous]
    public class HomeController : BaseApiController<HomeController>
    {
        public HomeController() { }

        // <All Active Category> => For User
        [HttpGet("[action]")]
        public async ValueTask<ActionResult> Category(CancellationToken cancellationToken)
        {
            var items = await Mediator.Send(new GetCategoriesForHomeQuery(), cancellationToken);
            return Ok(items);
        }
        [HttpGet("[action]")]
        public async ValueTask<ActionResult> Deals(CancellationToken cancellationToken)
        {
            var items = await Mediator.Send(new GetDealsForHomeQuery(), cancellationToken);
            return Ok(items);
        }
    }
}
