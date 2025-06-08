using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;

namespace UrlShortener.Data
{
    public class UrlShortenerContext : DbContext
    {
        public UrlShortenerContext(DbContextOptions<UrlShortenerContext> options) : base(options)
        {
        }

        public DbSet<Url> Urls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Url entity
            modelBuilder.Entity<Url>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.OriginalUrl)
                    .IsRequired()
                    .HasMaxLength(2048);
                
                entity.Property(e => e.ShortCode)
                    .IsRequired()
                    .HasMaxLength(20);
                
                entity.HasIndex(e => e.ShortCode)
                    .IsUnique()
                    .HasDatabaseName("IX_Urls_ShortCode");
                
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("datetime('now')");
            });
        }
    }
}