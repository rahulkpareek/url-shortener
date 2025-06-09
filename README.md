# URL Shortener

A simple and efficient URL shortener service built with ASP.NET Core Web API backend and vanilla JavaScript frontend. Transform your long URLs into short, shareable links with click tracking and management features.

## Features

- **URL Shortening**: Convert long URLs into short, manageable links
- **Auto-generated Short Codes**: 7-character alphanumeric codes for uniqueness
- **URL Redirection**: Redirect to original URLs using short codes
- **Click Tracking**: Monitor usage with click count statistics
- **URL Management**: View all shortened URLs with creation timestamps
- **Copy to Clipboard**: Easy sharing with one-click copy functionality
- **Responsive Design**: Works seamlessly across desktop and mobile devices
- **RESTful API**: Clean API endpoints for integration with other applications

### Nice to Have Features (Open for Contributions)

- **Custom Short Codes**: Allow users to create custom short codes instead of auto-generated ones
- **URL Analytics Dashboard**: Detailed analytics with charts showing click patterns, geographic data, and referrer information

## Tech Stack

### Backend
- **ASP.NET Core 6+** - Web API framework
- **Entity Framework Core** - ORM for database operations
- **SQLite** - Lightweight database for data persistence
- **Swagger/OpenAPI** - API documentation and testing

### Frontend
- **HTML5** - Semantic markup
- **CSS3** - Modern styling with responsive design
- **Vanilla JavaScript** - No framework dependencies
- **Fetch API** - HTTP client for API communication

## Getting Started

### Prerequisites
- .NET 6.0 SDK or later
- Any modern web browser
- Code editor (Visual Studio, VS Code, etc.)

### Backend Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/rahulkpareek/url-shortener.git
   cd url-shortener
   ```

2. **Navigate to the backend directory**
   ```bash
   cd UrlShortener
   ```

3. **Restore dependencies**
   ```bash
   dotnet restore
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

The API will be available at `http://localhost:5000` and Swagger documentation at `http://localhost:5000/swagger`.

### Frontend Setup

1. **Navigate to the frontend directory**
   ```bash
   cd frontend
   ```

2. **Open with a local server**
   
   Using Python:
   ```bash
   python -m http.server 8080
   ```
   
   Using Node.js (http-server):
   ```bash
   npx http-server -p 8080
   ```
   
   Or simply open `index.html` in your browser.

3. **Access the application**
   
   Open your browser and go to `http://localhost:8080`

## API Endpoints

### Base URL
```
http://localhost:5000/api/Url
```

### Endpoints

#### Create Short URL
```http
POST /shorten
Content-Type: application/json

{
  "originalUrl": "https://example.com/very/long/url"
}
```

**Response:**
```json
{
  "originalUrl": "https://example.com/very/long/url",
  "shortCode": "aBc123X",
  "shortUrl": "http://localhost:5000/aBc123X",
  "createdAt": "2024-01-15T10:30:00Z"
}
```

#### Get Original URL
```http
POST /geturl
Content-Type: application/json

{
  "shortCode": "aBc123X"
}
```

**Response:**
```json
{
  "originalUrl": "https://example.com/very/long/url"
}
```

#### Get All URLs
```http
GET /getall
```

**Response:**
```json
[
  {
    "originalUrl": "https://example.com/very/long/url",
    "shortCode": "aBc123X",
    "shortUrl": "http://localhost:5000/aBc123X",
    "createdAt": "2024-01-15T10:30:00Z"
  }
]
```

## Configuration

### Backend Configuration

The application uses `appsettings.json` for configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=urlshortener.db"
  },
  "BaseUrl": "http://localhost:5000"
}
```

### CORS Configuration

The API is configured to accept requests from common development ports. For production, update the CORS policy in `Program.cs` with your frontend domain.

## Database

The application uses SQLite with Entity Framework Core. The database is automatically created on first run with the following schema:

- **Urls Table**
  - Id (Primary Key)
  - OriginalUrl
  - ShortCode (Unique)
  - CreatedAt
  - ClickCount

## Usage

1. **Shorten a URL**: Enter your long URL in the input field and click "Shorten URL"
2. **Copy Short URL**: Use the copy button to copy the shortened URL to clipboard
3. **View Results**: See both the original and shortened URLs with creation timestamp

## Error Handling

The application includes comprehensive error handling:

- **Frontend**: User-friendly error messages for network issues and invalid inputs
- **Backend**: Proper HTTP status codes and error responses
- **Validation**: URL format validation and duplicate short code prevention

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/new-feature`)
3. Commit your changes (`git commit -am 'Add new feature'`)
4. Push to the branch (`git push origin feature/new-feature`)
5. Create a Pull Request

## License

This project is open source and available under the [MIT License](LICENSE).

## Author

**Rahul K Pareek** - [@rahulkpareek](https://github.com/rahulkpareek)

---

Built with ❤️ using ASP.NET Core and Vanilla JavaScript