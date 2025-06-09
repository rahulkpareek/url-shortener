using UrlShortener.Models;
namespace UrlShortener.Services
{
    public interface IUrlService
    {
        Task<ShortUrlResponse> CreateShortUrlAsync(string originalUrl);
        Task<string?> GetOriginalUrlAsync(string shortCode);
        Task<IEnumerable<ShortUrlResponse>> GetAllUrlsAsync();
    }
}