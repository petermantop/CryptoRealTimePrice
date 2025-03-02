using System.Net.Http.Headers;
using Newtonsoft.Json;
using CryptoRealtimePrice.Models;
public class TiingoClient
{
  private static readonly HttpClient _httpClient;

  private const string ApiKey = "";

  static TiingoClient()
  {
    _httpClient = new HttpClient();
    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {ApiKey}");
  }

  public async Task<decimal?> GetCryptoPriceAsync(string ticker)
  {
    var url = $"https://api.tiingo.com/tiingo/crypto/prices?tickers={ticker}";

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

    float? lastFloatPrice = priceDataList?.FirstOrDefault()?.PriceData?.LastOrDefault()?.Close;

    return lastFloatPrice.HasValue ? (decimal)lastFloatPrice.Value : null;
  }
}
