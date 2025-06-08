namespace UrlShortener.Models
{
    public class UrlResponse
    {
        public string OriginalUrl { get; set; } = string.Empty;
        public string ShortCode { get; set; } = string.Empty;
        public string ShortUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int ClickCount { get; set; }
    }
}