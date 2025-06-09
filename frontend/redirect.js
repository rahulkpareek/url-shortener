class UrlRedirector {
    constructor() {
        this.apiBaseUrl = 'http://localhost:5000';
        this.redirectTimer = null;
        this.countdownTimer = null;
        this.initializeEventListeners();
        this.checkUrlParams();
    }

    initializeEventListeners() {
        const form = document.getElementById('redirectForm');
        const visitBtn = document.getElementById('visitBtn');

        form.addEventListener('submit', (e) => this.handleSubmit(e));
        visitBtn.addEventListener('click', () => this.visitUrl());
    }

    checkUrlParams() {
        // Check if there's a short code in the URL parameters
        const urlParams = new URLSearchParams(window.location.search);
        const code = urlParams.get('code') || urlParams.get('c');
        
        if (code) {
            document.getElementById('shortCode').value = code;
            this.getOriginalUrl(code);
        }
    }

    async handleSubmit(event) {
        event.preventDefault();
        
        const shortCodeInput = document.getElementById('shortCode');
        const shortCode = shortCodeInput.value.trim();
        
        if (!shortCode) {
            this.showError('Please enter a short URL code');
            return;
        }

        await this.getOriginalUrl(shortCode);
    }

    async getOriginalUrl(shortCode) {
        this.setLoadingState(true);
        this.hideError();
        this.hideResult();
        this.clearTimers();

        try {
            const originalUrl = await this.fetchOriginalUrl(shortCode);
            this.showResult(originalUrl);
            this.startRedirectCountdown(originalUrl);
        } catch (error) {
            this.showError(error.message);
        } finally {
            this.setLoadingState(false);
        }
    }

    async fetchOriginalUrl(code) {
        try {
            const response = await fetch(`${this.apiBaseUrl}/api/Url/geturl`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    ShortCode: code
                })
            });

            if (!response.ok) {
                if (response.status === 404) {
                    throw new Error('Short URL not found. Please check the code and try again.');
                } else if (response.status === 400) {
                    throw new Error('Invalid short URL code provided');
                } else if (response.status === 405) {
                    throw new Error('Method not allowed. Please check the API endpoint.');
                } else if (response.status === 500) {
                    throw new Error('Server error occurred');
                } else {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
            }

            const data = await response.json();
            return data.originalUrl;
            
        } catch (error) {
            if (error.name === 'TypeError' && error.message.includes('fetch')) {
                throw new Error('Unable to connect to the server. Please check if the API is running.');
            }
            throw error;
        }
    }

    showResult(originalUrl) {
        const resultDiv = document.getElementById('result');
        const originalUrlInput = document.getElementById('originalUrl');

        originalUrlInput.value = originalUrl;
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

    startRedirectCountdown(url) {
        let countdown = 5;
        const countdownElement = document.getElementById('countdown');
        
        this.countdownTimer = setInterval(() => {
            countdown--;
            countdownElement.textContent = countdown;
            
            if (countdown <= 0) {
                this.redirectToUrl(url);
            }
        }, 1000);

        // Set a backup timer
        this.redirectTimer = setTimeout(() => {
            this.redirectToUrl(url);
        }, 5000);
    }

    visitUrl() {
        const originalUrl = document.getElementById('originalUrl').value;
        if (originalUrl) {
            this.clearTimers();
            this.redirectToUrl(originalUrl);
        }
    }

    redirectToUrl(url) {
        this.clearTimers();
        
        // Ensure URL has protocol
        if (!url.startsWith('http://') && !url.startsWith('https://')) {
            url = 'https://' + url;
        }
        
        window.location.href = url;
    }

    clearTimers() {
        if (this.redirectTimer) {
            clearTimeout(this.redirectTimer);
            this.redirectTimer = null;
        }
        
        if (this.countdownTimer) {
            clearInterval(this.countdownTimer);
            this.countdownTimer = null;
        }
    }
}

// Initialize the application when the DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    new UrlRedirector();
});

// Clean up timers when page is unloaded
window.addEventListener('beforeunload', () => {
    if (window.urlRedirector) {
        window.urlRedirector.clearTimers();
    }
});