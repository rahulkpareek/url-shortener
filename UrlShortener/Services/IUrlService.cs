using UrlShortener.Models;

namespace UrlShortener.Services
{
    public interface IUrlService
    {
        Task<UrlResponse> CreateShortUrlAsync(CreateUrlRequest request);
        Task<string?> GetOriginalUrlAsync(string shortCode);
        Task<UrlResponse?> GetUrlInfoAsync(string shortCode);
        Task<IEnumerable<UrlResponse>> GetAllUrlsAsync();
    }
}