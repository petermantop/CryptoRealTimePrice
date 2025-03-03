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
  public decimal Open { get; set; }
  public decimal High { get; set; }
  public decimal Low { get; set; }
  public decimal Close { get; set; }

  [JsonProperty("tradesDone")]
  public decimal TradesDone { get; set; }

  public decimal Volume { get; set; }
  public decimal VolumeNotional { get; set; }
}