using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models
{
    public class CreateUrlRequest
    {
        [Required]
        [Url]
        public string OriginalUrl { get; set; } = string.Empty;
    }
}