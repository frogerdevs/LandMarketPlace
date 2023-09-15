// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Subscription.Api.Controllers.v1.Brands
{
    [ApiVersion("1.0")]
    //[ApiExplorerSettings(GroupName = "Master")]
    public class BrandController : BaseApiController<BrandController>
    {
        private readonly ILogger<BrandController> _logger;
        public BrandController(ILogger<BrandController> logger)
        {
            _logger = logger;
        }
        // GET: api/<BrandController>
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Masuk nih guys");

            //var items = await Mediator.Send(new GetAllBrandQuery(), cancellationToken);

            return Ok();
        }

        // GET api/<BrandController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            //if (items is null || !items.Any())
            //    return NotFound();
            //else

            return "value";
        }

        // POST api/<BrandController>
        //[HttpPost]
        //public async Task<IActionResult> Post(AddBrandCommand command)
        //{
        //    return Ok(await Mediator.Send(command));
        //}

        // PUT api/<BrandController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BrandController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
