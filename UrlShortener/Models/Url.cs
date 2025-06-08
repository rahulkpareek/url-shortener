using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Models
{
    [Table("Urls")]
    public class Url
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(2048)]
        public string OriginalUrl { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(20)]
        public string ShortCode { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public int ClickCount { get; set; } = 0;
        
        public DateTime? LastAccessedAt { get; set; }
    }
}