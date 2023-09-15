using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Gateway.Controllers.v1
{
    [ApiVersion("1.0")]
    public class DistrictController : BaseApiController<DistrictController>
    {
        private readonly ILogger<DistrictController> _logger;
        readonly IDistrictService _districtService;
        public DistrictController(ILogger<DistrictController> logger, IDistrictService districtService)
        {
            _logger = logger;
            _districtService = districtService;
        }
        // GET: api/<DistrictController>
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            var httpResponse = await _districtService.GetDistrictsAsync(cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // GET api/<DistrictController>/5
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            var httpResponse = await _districtService.GetDistrictByIdAsync(id, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
        [HttpGet("[action]/{provinceid}")]
        public async ValueTask<ActionResult> ByCity(string provinceid, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(provinceid))
            {
                return BadRequest();
            }
            var httpResponse = await _districtService.GetDistrictByCityAsync(provinceid, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }


        //// POST api/<DistrictController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<DistrictController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<DistrictController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
