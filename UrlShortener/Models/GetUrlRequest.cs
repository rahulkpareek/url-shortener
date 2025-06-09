using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models
{
    public class GetUrlRequest
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Short code must be at most 10 characters long.")]
        public string ShortCode { get; set; } = string.Empty;
    }
}