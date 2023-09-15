using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityServer.Controllers.Api.v1
{
    [ApiVersion("1.0")]
    public class ProvinceController : BaseApiController<ProvinceController>
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<ProvinceController> _logger;
        private readonly string _keyProvince = "AllProvince";
        public ProvinceController(ILogger<ProvinceController> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }
        [HttpGet]
        public async ValueTask<ActionResult> Get(CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(_keyProvince, out object? existingProvince)
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

            var provincies = await Mediator.Send(new GetProvinciesQuery(), cancellationToken);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(30)); // Atur waktu kadaluwarsa cache
            _cache.Set(_keyProvince, provincies.Data, cacheEntryOptions);

            return Ok(provincies);
        }
        [HttpGet("{id}")]
        public async ValueTask<ActionResult> Get(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            var province = await Mediator.Send(new GetProvinceByIdQuery() { Id = id }, cancellationToken);
            if (province == null)
            {
                return NotFound();
            }
            return Ok(new BaseWithDataResponse { Success = true, Message = "Success Get Data", Data = province });
        }
    }
}
