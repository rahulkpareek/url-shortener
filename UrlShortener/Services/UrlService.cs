using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Models;

namespace UrlShortener.Services
{
    public class UrlService : IUrlService
    {
        private readonly UrlShortenerContext _context;
        private readonly IConfiguration _configuration;

        public UrlService(UrlShortenerContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ShortUrlResponse> CreateShortUrlAsync(string originalUrl)
        {
            // Generate short code in backend
            var shortCode = GenerateShortCode();

            // Check if short code already exists
            if (await _context.Urls.AnyAsync(u => u.ShortCode == shortCode))
            {
                throw new InvalidOperationException("Short code already exists");
            }

            var url = new Url
            {
                OriginalUrl = originalUrl,
                ShortCode = shortCode,
                CreatedAt = DateTime.UtcNow,
                ClickCount = 0
            };

            _context.Urls.Add(url);
            await _context.SaveChangesAsync();

            var baseUrl = _configuration["BaseUrl"] ?? "http://localhost:5000";
            var response = new ShortUrlResponse
            {
                OriginalUrl = url.OriginalUrl,
                ShortCode = url.ShortCode,
                ShortUrl = $"{baseUrl}/{url.ShortCode}",
                CreatedAt = url.CreatedAt
            };

            return response;
        }

        public async Task<string?> GetOriginalUrlAsync(string shortCode)
        {
            var url = await _context.Urls.FirstOrDefaultAsync(u => u.ShortCode == shortCode);
            return url?.OriginalUrl;
        }

        private string GenerateShortCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 7)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<IEnumerable<ShortUrlResponse>> GetAllUrlsAsync()
        {
            return await _context.Urls.Select(url => new ShortUrlResponse
            {
                OriginalUrl = url.OriginalUrl,
                ShortCode = url.ShortCode,
                ShortUrl = $"{_configuration["BaseUrl"]}/{url.ShortCode}",
                CreatedAt = url.CreatedAt
            }).ToListAsync();
        }
    }
}