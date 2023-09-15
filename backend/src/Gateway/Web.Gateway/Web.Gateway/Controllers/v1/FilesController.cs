using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Web.Gateway.Controllers.v1
{
    [ApiVersion("1.0")]
    public class FilesController : BaseApiController<UserController>
    {
        private readonly IMemoryCache _memoryCache;
        readonly IWebHostEnvironment _environment;

        public FilesController(IMemoryCache memoryCache, IWebHostEnvironment environment)
        {
            _memoryCache = memoryCache;
            _environment = environment;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid().ToString("N")}_{Path.GetFileName(file.FileName)}";
                    var filePath = Path.Combine(_environment.ContentRootPath, "Files", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Tambahkan file ke dalam cache
                    var cacheEntryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache selama 30 menit
                    };
                    _memoryCache.Set(fileName, filePath, cacheEntryOptions);

                    return Ok(new { FileName = fileName });
                }

                return BadRequest(new { message = "Tidak ada file yang diunggah." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Terjadi kesalahan: {ex.Message}" });
            }
        }

        [HttpGet("{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            if (_memoryCache.TryGetValue(fileName, out string filePath))
            {
                var fileStream = new FileStream(filePath, FileMode.Open);
                return File(fileStream, "application/octet-stream", fileName);
            }

            var localPath = Path.Combine(_environment.ContentRootPath, "Files", fileName);


            var imageBytes = await System.IO.File.ReadAllBytesAsync(localPath);
            if (imageBytes != null)
            {
                // Simpan file ke cache untuk penggunaan selanjutnya
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30)); // Atur waktu kadaluwarsa cache
                _memoryCache.Set(fileName, localPath, cacheEntryOptions);
                return File(imageBytes, "application/octet-stream", fileName);
            }

            return NotFound(new { message = "File tidak ditemukan." });
        }
    }
}
