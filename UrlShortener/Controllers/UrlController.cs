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
        public async Task<ActionResult<UrlResponse>> CreateShortUrl([FromBody] CreateUrlRequest request)
        {
            try
            {
                var result = await _urlService.CreateShortUrlAsync(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{shortCode}")]
        public async Task<ActionResult<UrlResponse>> GetUrlInfo(string shortCode)
        {
            var result = await _urlService.GetUrlInfoAsync(shortCode);
            if (result == null)
            {
                return NotFound("Short URL not found");
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UrlResponse>>> GetAllUrls()
        {
            var result = await _urlService.GetAllUrlsAsync();
            return Ok(result);
        }
    }
}