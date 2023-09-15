using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityServer.Controllers.Api.v1
{
    public class CityController : BaseApiController<CityController>
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<CityController> _logger;
        private readonly string _keyCities = "Cities";
        private readonly string _keyCityByProvince = "CityByProvince";
        public CityController(ILogger<CityController> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }
        // GET: api/<UserController>
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(_keyCities, out object? existingCity)
                && existingCity is List<Tuple<string, string, string>> listCached)
            {
                //int itemCount = 0;
                //if (existingCity is List <Tuple<string, string>> kelurahanListCached)
                //{
                //    itemCount = kelurahanListCached.Count;
                //    return Ok(itemCount);
                //}
                // data ditemukan di cache
                return Ok(new BaseWithDataCountResponse
                {
                    Success = true,
                    Message = "Success Get Data",
                    Count = listCached.Count,
                    Data = existingCity
                });
            }

            var cities = await Mediator.Send(new GetCitiesQuery(), cancellationToken);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(30)); // Atur waktu kadaluwarsa cache
            _cache.Set(_keyCities, cities.Data, cacheEntryOptions);

            return Ok(cities);
        }
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            var city = await Mediator.Send(new GetCityByIdQuery() { Id = id }, cancellationToken);
            if (city == null)
            {
                return NotFound();
            }
            return Ok(new BaseWithDataResponse { Success = true, Message = "Success Get Data", Data = city });
        }
        [HttpGet("[action]/{provinceid}")]
        public async ValueTask<ActionResult> ByProvince(string provinceid, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(provinceid))
            {
                return BadRequest();
            }
            if (_cache.TryGetValue($"{_keyCityByProvince}_{provinceid}", out object? existingCity))
            {
                // data ditemukan di cache
                return Ok(new BaseWithDataResponse
                {
                    Success = true,
                    Message = "Success Get Data",
                    Data = existingCity
                });
            }

            var cities = await Mediator.Send(new GetCityByProvinceIdQuery() { ProvinceId = provinceid }, cancellationToken);
            if (cities == null)
            {
                return NotFound();
            }
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                      .SetSlidingExpiration(TimeSpan.FromMinutes(30)); // Atur waktu kadaluwarsa cache
            _cache.Set($"{_keyCityByProvince}_{provinceid}", cities.Data, cacheEntryOptions);

            return Ok(cities);
        }
    }
}
