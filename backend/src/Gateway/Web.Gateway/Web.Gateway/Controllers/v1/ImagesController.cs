using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Gateway.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class ImagesController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        readonly IWebHostEnvironment _environment;
        public ImagesController(IMemoryCache cache, IWebHostEnvironment environment)
        {
            _cache = cache;
            _environment = environment;
        }
        // GET: api/<ImagesController>
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(string key)
        {
            try
            {
                string fileExtension = string.Empty;
                string mediaType = string.Empty;
                // Cek apakah gambar sudah ada di cache
                if (_cache.TryGetValue(key, out byte[]? existingImageBytes))
                {
                    // Gambar ditemukan di cache, kirimkan sebagai respons
                    fileExtension = Path.GetExtension(key);
                    mediaType = GetMediaType(fileExtension);
                    return File(existingImageBytes!, mediaType);
                }


                var imagePath = Path.Combine(_environment.ContentRootPath, "Pict", key);
                fileExtension = Path.GetExtension(imagePath);

                mediaType = GetMediaType(fileExtension);

                if (mediaType == null)
                {
                    return BadRequest("Tipe file gambar tidak didukung");
                }

                var imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
                if (imageBytes != null)
                {
                    // Simpan gambar ke cache untuk penggunaan selanjutnya
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(30)); // Atur waktu kadaluwarsa cache
                    _cache.Set(key, imageBytes, cacheEntryOptions);
                    return File(imageBytes, mediaType);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed fetch image");
            }

        }



        //// POST api/<ImagesController>
        //[HttpPost]
        //[Consumes("multipart/form-data")]
        //public async Task<IActionResult> Post([FromBody] ImagesRequest model)
        //{
        //    try
        //    {

        //        // Validasi model, pastikan model berisi file gambar
        //        if (model == null || model.Image == null || model.Image.Length == 0)
        //        {
        //            return BadRequest("Invalid image data");
        //        }

        //        // Lakukan pemrosesan gambar (misalnya penyimpanan ke penyimpanan berbasis objek)
        //        var fileExtension = Path.GetExtension(model.Image.FileName)?.TrimStart('.');
        //        var uniqueFileName = Guid.NewGuid().ToString("N") + "." + fileExtension;
        //        var filePath = Path.Combine(_environment.ContentRootPath, "Pict", uniqueFileName);

        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await model.Image.CopyToAsync(stream);
        //        }

        //        // Menghapus cache jika ada
        //        _cache.Remove(uniqueFileName);

        //        // Simpan gambar ke cache untuk penggunaan selanjutnya
        //        var imageBytes = await System.IO.File.ReadAllBytesAsync(filePath);
        //        var cacheEntryOptions = new MemoryCacheEntryOptions()
        //            .SetSlidingExpiration(TimeSpan.FromMinutes(30)); // Atur waktu kadaluwarsa cache
        //        _cache.Set(uniqueFileName, imageBytes, cacheEntryOptions);



        //        return Ok(new { FileName = uniqueFileName });
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, "Failed Upload Image");
        //    }
        //}
        [HttpPost()]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> Post(IFormFile file)
        {
            try
            {

                // Validasi model, pastikan model berisi file gambar
                if (file == null || file.Length == 0)
                {
                    return BadRequest("Invalid image data");
                }

                // Lakukan pemrosesan gambar (misalnya penyimpanan ke penyimpanan berbasis objek)
                var fileExtension = Path.GetExtension(file.FileName)?.TrimStart('.');
                var uniqueFileName = Guid.NewGuid().ToString("N") + "." + fileExtension;
                var filePath = Path.Combine(_environment.ContentRootPath, "Pict", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Menghapus cache jika ada
                _cache.Remove(uniqueFileName);

                // Simpan gambar ke cache untuk penggunaan selanjutnya
                var imageBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30)); // Atur waktu kadaluwarsa cache
                _cache.Set(uniqueFileName, imageBytes, cacheEntryOptions);



                return Ok(new { FileName = uniqueFileName });
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed Upload Image");
            }
        }

        // PUT api/<ImagesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ImagesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        static string GetMediaType(string extension) => extension switch
        {
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".bmp" => "image/bmp",
            ".tiff" => "image/tiff",
            ".wmf" => "image/wmf",
            ".jp2" => "image/jp2",
            ".svg" => "image/svg+xml",
            _ => "application/octet-stream",
        };

    }
}
