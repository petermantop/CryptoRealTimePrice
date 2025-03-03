using System.Net.Http.Headers;
using Newtonsoft.Json;
using CryptoRealtimePrice.Models;

public class TiingoRestClientService
{
  private static readonly HttpClient _httpClient = new HttpClient();
  private readonly string _apiKey;

  public TiingoRestClientService(AppSettings appSettings)
  {
    _apiKey = appSettings.TiingoApiKey;

    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {_apiKey}");
  }

  public async Task<string?> GetCryptoPriceAsync(string ticker)
  {
    var url = $"https://api.tiingo.com/tiingo/crypto/prices?tickers={ticker}&includeRawExchangeData=true";

    using var request = new HttpRequestMessage(HttpMethod.Get, url);
    var response = await _httpClient.SendAsync(request);

    if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
    {
      throw new UnauthorizedAccessException("Access denied. API key might be invalid.");
    }

    if (!response.IsSuccessStatusCode)
    {
      throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
    }

    var jsonResponse = await response.Content.ReadAsStringAsync();
    var priceDataList = JsonConvert.DeserializeObject<List<TiingoPriceData>>(jsonResponse);

    if (priceDataList == null || priceDataList.Count == 0)
    {
      return null;
    }

    var latestPriceData = priceDataList.FirstOrDefault()?.PriceData?.LastOrDefault();

    if (latestPriceData == null)
    {
      return null;
    }

    // Use Mid Price for Market Price Calculation
    decimal midPrice = (latestPriceData.High + latestPriceData.Low) / 2;
    string price = midPrice.ToString("F2");
    return price;
  }
}
