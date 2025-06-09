using Microsoft.AspNetCore.Mvc;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlController : ControllerBase
    {
        private readonly IUrlService _urlService;

        public UrlController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        [HttpPost("shorten")]
        public async Task<IActionResult> CreateShortUrl([FromBody] CreateUrlRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Remove any reference to request.CustomShortCode
                var shortUrl = await _urlService.CreateShortUrlAsync(request.OriginalUrl);
                return Ok(shortUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("geturl")]
        public async Task<IActionResult> GetUrl([FromBody] GetUrlRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var originalUrl = await _urlService.GetOriginalUrlAsync(request.ShortCode);
                
                if (originalUrl == null)
                {
                    return NotFound("Short code not found");
                }

                return Ok(new { OriginalUrl = originalUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllUrls()
        {
            try
            {
                var urls = await _urlService.GetAllUrlsAsync();
                return Ok(urls);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}