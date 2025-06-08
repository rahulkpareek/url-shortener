using UrlShortener.Models;
namespace UrlShortener.Services
{
    public interface IUrlService
    {
        Task<ShortUrlResponse> CreateShortUrlAsync(string originalUrl);
        Task<IEnumerable<ShortUrlResponse>> GetAllUrlsAsync();
    }
}