using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Services
{
    public class UrlService : IUrlService
    {
        private readonly UrlShortenerContext _context;
        private readonly Random _random = new();
        private readonly IConfiguration _configuration;

        public UrlService(UrlShortenerContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<UrlResponse> CreateShortUrlAsync(CreateUrlRequest request)
        {
            var shortCode = !string.IsNullOrEmpty(request.CustomShortCode) 
                ? request.CustomShortCode 
                : await GenerateUniqueShortCodeAsync();

            // Check if short code already exists
            if (await _context.Urls.AnyAsync(u => u.ShortCode == shortCode))
            {
                throw new InvalidOperationException("Short code already exists");
            }

            var url = new Url
            {
                OriginalUrl = request.OriginalUrl,
                ShortCode = shortCode,
                CreatedAt = DateTime.UtcNow
            };

            _context.Urls.Add(url);
            await _context.SaveChangesAsync();

            var baseUrl = _configuration["BaseUrl"] ?? "https://localhost:5001";
            var response = new UrlResponse
            {
                OriginalUrl = url.OriginalUrl,
                ShortCode = url.ShortCode,
                ShortUrl = $"{baseUrl}/{url.ShortCode}",
                CreatedAt = url.CreatedAt,
                ClickCount = url.ClickCount
            };

            return response;
        }

        public async Task<string?> GetOriginalUrlAsync(string shortCode)
        {
            var url = await _context.Urls.FirstOrDefaultAsync(u => u.ShortCode == shortCode);
            if (url != null)
            {
                url.ClickCount++;
                url.LastAccessedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return url.OriginalUrl;
            }
            return null;
        }

        public async Task<UrlResponse?> GetUrlInfoAsync(string shortCode)
        {
            var url = await _context.Urls.FirstOrDefaultAsync(u => u.ShortCode == shortCode);
            if (url == null) return null;

            var baseUrl = _configuration["BaseUrl"] ?? "https://localhost:5001";
            var response = new UrlResponse
            {
                OriginalUrl = url.OriginalUrl,
                ShortCode = url.ShortCode,
                ShortUrl = $"{baseUrl}/{url.ShortCode}",
                CreatedAt = url.CreatedAt,
                ClickCount = url.ClickCount
            };

            return response;
        }

        public async Task<IEnumerable<UrlResponse>> GetAllUrlsAsync()
        {
            var urls = await _context.Urls.OrderByDescending(u => u.CreatedAt).ToListAsync();
            var baseUrl = _configuration["BaseUrl"] ?? "https://localhost:5001";
            
            var responses = urls.Select(url => new UrlResponse
            {
                OriginalUrl = url.OriginalUrl,
                ShortCode = url.ShortCode,
                ShortUrl = $"{baseUrl}/{url.ShortCode}",
                CreatedAt = url.CreatedAt,
                ClickCount = url.ClickCount
            });

            return responses;
        }

        private async Task<string> GenerateUniqueShortCodeAsync()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string shortCode;
            
            do
            {
                shortCode = new string(Enumerable.Repeat(chars, 7)
                    .Select(s => s[_random.Next(s.Length)]).ToArray());
            }
            while (await _context.Urls.AnyAsync(u => u.ShortCode == shortCode));
            
            return shortCode;
        }
    }
}