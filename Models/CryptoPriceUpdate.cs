using Newtonsoft.Json;
using System;

namespace CryptoRealtimePrice.Models
{
  public class CryptoPriceUpdate
  {
    [JsonProperty("service")]
    public string Service { get; set; } = string.Empty;

    [JsonProperty("messageType")]
    public string MessageType { get; set; } = string.Empty;

    [JsonProperty("data")]
    public required object[] Data { get; set; }
  }

  public class CryptoTradeUpdate
  {
    public required string Ticker { get; set; }
    public DateTime Date { get; set; }
    public required string Exchange { get; set; }
    public decimal LastSize { get; set; }
    public decimal LastPrice { get; set; }
  }

  public class CryptoQuoteUpdate
  {
    public required string Ticker { get; set; }
    public DateTime Date { get; set; }
    public required string Exchange { get; set; }
    public decimal BidSize { get; set; }
    public decimal BidPrice { get; set; }
    public decimal MidPrice { get; set; }
    public decimal AskSize { get; set; }
    public decimal AskPrice { get; set; }
  }

  public class CryptoSubscriptionResponse
  {
    [JsonProperty("service")]
    public string Service { get; set; } = string.Empty;

    [JsonProperty("messageType")]
    public string MessageType { get; set; } = string.Empty;

    [JsonProperty("data")]
    public SubscriptionData Data { get; set; } = new SubscriptionData();
  }

  public class SubscriptionData
  {
    [JsonProperty("subscriptionId")]
    public string SubscriptionId { get; set; } = string.Empty;
  }
}
