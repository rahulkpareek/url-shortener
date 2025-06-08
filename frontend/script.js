class UrlShortener {
    constructor() {
        this.apiBaseUrl = 'https://localhost:5000'; 
        this.initialize();
        this.initializeEventListeners();
    }

    initializeEventListeners() {
        const form = document.getElementById('urlForm');
        const copyBtn = document.getElementById('copyBtn');

        form.addEventListener('submit', (e) => this.handleSubmit(e));
        copyBtn.addEventListener('click', () => this.copyToClipboard());
    }

    async handleSubmit(event) {
        event.preventDefault();
        
        const longUrlInput = document.getElementById('longUrl');
        const longUrl = longUrlInput.value.trim();
        
        if (!longUrl) {
            this.showError('Please enter a valid URL');
            return;
        }

        this.setLoadingState(true);
        this.hideError();
        this.hideResult();

        try {
            const shortUrl = await this.shortenUrl(longUrl);
            this.showResult(longUrl, shortUrl);
            longUrlInput.value = ''; // Clear the input
        } catch (error) {
            this.showError(error.message);
        } finally {
            this.setLoadingState(false);
        }
    }

    async shortenUrl(longUrl) {
        try {
            const response = await fetch(`${this.apiBaseUrl}/api/Url/shorten`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ originalUrl: longUrl })
            });

            if (!response.ok) {
                if (response.status === 400) {
                    throw new Error('Invalid URL provided');
                } else if (response.status === 500) {
                    throw new Error('Server error occurred');
                } else {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
            }

            const data = await response.json();
            
            // Assuming your API returns the short URL in a specific format
            // Adjust this based on your actual API response structure
            return data.shortUrl || data.url || `${this.apiBaseUrl}/${data.code}`;
            
        } catch (error) {
            if (error.name === 'TypeError' && error.message.includes('fetch')) {
                throw new Error('Unable to connect to the server. Please check if the API is running.');
            }
            throw error;
        }
    }

    showResult(originalUrl, shortUrl) {
        const resultDiv = document.getElementById('result');
        const shortUrlInput = document.getElementById('shortUrl');
        const originalUrlSpan = document.getElementById('originalUrl');

        shortUrlInput.value = shortUrl;
        originalUrlSpan.textContent = originalUrl;
        resultDiv.style.display = 'block';
    }

    hideResult() {
        const resultDiv = document.getElementById('result');
        resultDiv.style.display = 'none';
    }

    showError(message) {
        const errorDiv = document.getElementById('error');
        const errorMessage = document.getElementById('errorMessage');
        
        errorMessage.textContent = message;
        errorDiv.style.display = 'block';
    }

    hideError() {
        const errorDiv = document.getElementById('error');
        errorDiv.style.display = 'none';
    }

    setLoadingState(isLoading) {
        const submitBtn = document.querySelector('.shorten-btn');
        const btnText = document.querySelector('.btn-text');
        const btnLoading = document.querySelector('.btn-loading');

        if (isLoading) {
            submitBtn.disabled = true;
            btnText.style.display = 'none';
            btnLoading.style.display = 'inline';
        } else {
            submitBtn.disabled = false;
            btnText.style.display = 'inline';
            btnLoading.style.display = 'none';
        }
    }

    async copyToClipboard() {
        const shortUrlInput = document.getElementById('shortUrl');
        const copyBtn = document.getElementById('copyBtn');
        
        try {
            await navigator.clipboard.writeText(shortUrlInput.value);
            
            // Visual feedback
            const originalText = copyBtn.textContent;
            copyBtn.textContent = 'Copied!';
            copyBtn.classList.add('copied');
            
            setTimeout(() => {
                copyBtn.textContent = originalText;
                copyBtn.classList.remove('copied');
            }, 2000);
            
        } catch (error) {
            // Fallback for older browsers
            shortUrlInput.select();
            document.execCommand('copy');
            
            copyBtn.textContent = 'Copied!';
            setTimeout(() => {
                copyBtn.textContent = 'Copy';
            }, 2000);
        }
    }
}

// Initialize the application when the DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    new UrlShortener();
});