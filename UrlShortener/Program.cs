using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://127.0.0.1:3000", "http://localhost:8080", "http://127.0.0.1:8080")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
    
    // Alternative: Allow all origins for development (less secure)
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add SQLite Database
builder.Services.AddDbContext<UrlShortenerContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                     "Data Source=urlshortener.db"));

// Add Swagger services
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "URL Shortener API",
        Version = "v1",
        Description = "A simple URL shortener service"
    });
    c.EnableAnnotations();
});

builder.Services.AddScoped<IUrlService, UrlService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Create database and apply migrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<UrlShortenerContext>();
    context.Database.EnsureCreated();
}

// IMPORTANT: Configure CORS middleware BEFORE other middleware
app.UseCors("AllowFrontend"); // Use "AllowAll" for development if needed

// Configure Swagger middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "URL Shortener API V1");
    c.RoutePrefix = "swagger"; // Makes Swagger available at /swagger
    c.DocumentTitle = "URL Shortener API Documentation";
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Redirect endpoint
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
