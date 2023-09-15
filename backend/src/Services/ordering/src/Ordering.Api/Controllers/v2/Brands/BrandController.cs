// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ordering.Api.Controllers.v2.Brands
{
    [ApiVersion("2.0")]
    public class BrandController : BaseApiController<BrandController>
    {
        // GET: api/<BrandController>
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            //var items = await Mediator.Send(new GetAllBrandQuery(), cancellationToken);

            //if (items is null || !items.Any())
            //    return NotFound();
            //else
            return Ok();
        }

        // GET api/<BrandController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<BrandController>
        [HttpPost]
        public async Task<IActionResult> Post(string command)
        {
            return Ok(await Mediator.Send(command));
        }

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
