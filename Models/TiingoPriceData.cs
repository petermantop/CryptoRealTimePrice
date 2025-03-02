namespace CryptoRealtimePrice.Models;
using Newtonsoft.Json;

public class TiingoPriceData
{
  public required string Ticker { get; set; }
  public required string BaseCurrency { get; set; }
  public required string QuoteCurrency { get; set; }
  public required List<PriceData> PriceData { get; set; }
}

public class PriceData
{
  public DateTime Date { get; set; }
  public float Open { get; set; }
  public float High { get; set; }
  public float Low { get; set; }
  public float Close { get; set; }

  [JsonProperty("tradesDone")]
  public decimal TradesDone { get; set; }

  public float Volume { get; set; }
  public float VolumeNotional { get; set; }
}