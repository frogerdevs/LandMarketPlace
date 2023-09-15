using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityServer.Controllers.Api.v1
{
    [ApiVersion("1.0")]
    public class SubDistrictController : BaseApiController<SubDistrictController>
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<SubDistrictController> _logger;
        private readonly string _keySubDistrict = "AllSubDistrict";
        private readonly string _keySubDistrictByCity = "SubDistrictByDistrict";
        public SubDistrictController(ILogger<SubDistrictController> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }
        // GET: api/<UserController>
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(_keySubDistrict, out object? existingProvince)
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

            var subdistricts = await Mediator.Send(new GetSubDistrictsQuery(), cancellationToken);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(30)); // Atur waktu kadaluwarsa cache
            _cache.Set(_keySubDistrict, subdistricts.Data, cacheEntryOptions);

            return Ok(subdistricts);
        }
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            var subdistrict = await Mediator.Send(new GetSubDistrictByIdQuery() { Id = id }, cancellationToken);
            if (subdistrict == null)
            {
                return NotFound();
            }
            return Ok(new BaseWithDataResponse { Success = true, Message = "Success Get Data", Data = subdistrict });
        }
        [HttpGet("[action]/{districtid}")]
        public async ValueTask<ActionResult> ByDistrict(string districtid, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(districtid))
            {
                return BadRequest();
            }

            if (_cache.TryGetValue($"{_keySubDistrictByCity}_{districtid}", out object? existingSubdistricts))
            {
                // data ditemukan di cache
                return Ok(new BaseWithDataResponse
                {
                    Success = true,
                    Message = "Success Get Data",
                    Data = existingSubdistricts
                });
            }

            var subdistricts = await Mediator.Send(new GetSubDistrictByDistrictIdQuery() { DistrictId = districtid }, cancellationToken);
            if (subdistricts == null)
            {
                return NotFound();
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30)); // Atur waktu kadaluwarsa cache
            _cache.Set($"{_keySubDistrictByCity}_{districtid}", subdistricts.Data, cacheEntryOptions);
            return Ok(subdistricts);
        }
    }
}
