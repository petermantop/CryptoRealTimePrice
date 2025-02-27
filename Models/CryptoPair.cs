namespace CryptoRealtimePrice.Models
{
  public class CryptoPair
  {
    public string Symbol { get; set; }
    public string Name { get; set; }

    public CryptoPair(string symbol, string name)
    {
      Symbol = symbol;
      Name = name;
    }
  }
}