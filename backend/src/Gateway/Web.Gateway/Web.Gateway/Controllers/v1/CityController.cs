using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Gateway.Controllers.v1
{
    [ApiVersion("1.0")]
    public class CityController : BaseApiController<ProvinceController>
    {
        private readonly ILogger<ProvinceController> _logger;
        readonly ICityService _cityService;
        public CityController(ILogger<ProvinceController> logger, ICityService cityService)
        {
            _logger = logger;
            _cityService = cityService;
        }
        // GET: api/<CityController>
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            var httpResponse = await _cityService.GetCitiesAsync(cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // GET api/<CityController>/5
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            var httpResponse = await _cityService.GetCityByIdAsync(id, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
        [HttpGet("[action]/{provinceid}")]
        public async ValueTask<ActionResult> ByProvince(string provinceid, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(provinceid))
            {
                return BadRequest();
            }
            var httpResponse = await _cityService.GetCityByProvinceAsync(provinceid, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        //// POST api/<CityController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<CityController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<CityController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
