using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Gateway.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ProvinceController : BaseApiController<ProvinceController>
    {
        private readonly ILogger<ProvinceController> _logger;
        readonly IProvinceService _provinceService;
        public ProvinceController(ILogger<ProvinceController> logger, IProvinceService provinceService)
        {
            _logger = logger;
            _provinceService = provinceService;
        }
        // GET: api/<ProvinceController>
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            var httpResponse = await _provinceService.GetProvinciesAsync(cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        // GET api/<ProvinceController>/5
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            var httpResponse = await _provinceService.GetProvinceByIdAsync(id, cancellationToken);
            return await httpResponse.ToActionResultAsync(cancellationToken);
        }

        //// POST api/<ProvinceController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<ProvinceController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<ProvinceController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
