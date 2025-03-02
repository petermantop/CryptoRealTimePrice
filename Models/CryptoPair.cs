namespace CryptoRealtimePrice.Models
{
  public class CryptoPair
  {
    public string Ticker { get; set; }
    public string Name { get; set; }

    public CryptoPair(string ticker, string name)
    {
      Ticker = ticker;
      Name = name;
    }
  }
}