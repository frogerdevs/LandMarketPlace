using Catalog.Application.Features.Weather.Queries;

namespace Catalog.Api.Controllers
{
    [ApiVersion("1.0")]
    //[ApiExplorerSettings(GroupName = "Master")]
    public class WeatherForecastController : BaseApiController<WeatherForecastController>
    {

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async ValueTask<ActionResult> GetAsync(CancellationToken cancellationToken)
        {
            var items = await Mediator.Send(new GetForeCastRequest(), cancellationToken);

            if (items is null || !items.Any())
                return NoContent();
            else
                return Ok(items);

        }
    }
}