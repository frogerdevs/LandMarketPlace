using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Gateway.Controllers.v1
{
    [ApiVersion("1.0")]
    public class SubDistrictController : BaseApiController<SubDistrictController>
    {
        private readonly ILogger<SubDistrictController> _logger;
        readonly ISubDistrictService _subdistrictService;
        public SubDistrictController(ILogger<SubDistrictController> logger, ISubDistrictService subdistrictService)
        {
            _logger = logger;
            _subdistrictService = subdistrictService;
        }
        // GET: api/<SubDistrictController>
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            var httpResponse = await _subdistrictService.GetSubDistrictsAsync(cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // GET api/<SubDistrictController>/5
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            var httpResponse = await _subdistrictService.GetSubDistrictByIdAsync(id, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }
        [HttpGet("[action]/{districtid}")]
        public async ValueTask<ActionResult> ByDistrict(string districtid, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(districtid))
            {
                return BadRequest();
            }
            var httpResponse = await _subdistrictService.GetSubDistrictByDistrictAsync(districtid, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        //// POST api/<SubDistrictController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<SubDistrictController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<SubDistrictController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
