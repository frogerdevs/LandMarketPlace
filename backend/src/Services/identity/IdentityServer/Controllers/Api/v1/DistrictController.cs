using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityServer.Controllers.Api.v1
{
    [ApiVersion("1.0")]
    public class DistrictController : BaseApiController<AuthController>
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<DistrictController> _logger;
        private readonly string _keyDistrict = "AllDistrict";
        private readonly string _keyDistrictByCity = "DistrictByCity";
        public DistrictController(ILogger<DistrictController> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }
        // GET: api/<UserController>
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(_keyDistrict, out object? existingProvince)
                && existingProvince is List<Tuple<string, string, string>> listCached)
            {
                // data ditemukan di cache
                return Ok(new BaseWithDataCountResponse
                {
                    Success = true,
                    Message = "Success Get Data",
                    Count = listCached.Count,
                    Data = existingProvince
                });
            }

            var districts = await Mediator.Send(new GetDistrictsQuery(), cancellationToken);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(30)); // Atur waktu kadaluwarsa cache
            _cache.Set(_keyDistrict, districts.Data, cacheEntryOptions);

            return Ok(districts);
        }
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            var district = await Mediator.Send(new GetDistrictByIdQuery() { Id = id }, cancellationToken);
            if (district == null)
            {
                return NotFound();
            }
            return Ok(new BaseWithDataResponse { Success = true, Message = "Success Get Data", Data = district });
        }
        [HttpGet("[action]/{cityid}")]
        public async ValueTask<ActionResult> ByCity(string cityid, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(cityid))
            {
                return BadRequest();
            }
            if (_cache.TryGetValue($"{_keyDistrictByCity}_{cityid}", out object? existingDistricts))
            {
                // data ditemukan di cache
                return Ok(new BaseWithDataResponse
                {
                    Success = true,
                    Message = "Success Get Data",
                    Data = existingDistricts
                });
            }

            var districts = await Mediator.Send(new GetDistrictByCityIdQuery() { CityId = cityid }, cancellationToken);
            if (districts == null)
            {
                return NotFound();
            }
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30)); // Atur waktu kadaluwarsa cache
            _cache.Set($"{_keyDistrictByCity}_{cityid}", districts.Data, cacheEntryOptions);

            return Ok(districts);
        }
    }
}
