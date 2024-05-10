window.searchGoogleImages = async (cityName) => {
    const cx = 'b3c6c1426b5dd4fb3';
    const apiKey = 'AIzaSyDQ6fXVvOySDENcpUrAJdDt0xFfBFxOcTY';
    const query = `${cityName} city`;

    const url = `https://www.googleapis.com/customsearch/v1?q=${query}&cx=${cx}&searchType=image&key=${apiKey}`;
    const response = await fetch(url);
    const data = await response.json();

    if (data.items && data.items.length > 0) {
        return data.items[0].link;
    }

    return null;
};

