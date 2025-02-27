using CryptoRealtimePrice.Models;

namespace CryptoRealtimePrice.Services
{
  public class CryptoPairService
  {
    // Hardcoded list of available crypto pairs
    private static readonly List<CryptoPair> AvailableCryptoPairs = new List<CryptoPair>
        {
            new CryptoPair("BTCUSD", "Bitcoin to USD"),
            new CryptoPair("ETHUSD", "Ethereum to USD"),
            new CryptoPair("XRPUSD", "Ripple to USD"),
            new CryptoPair("LTCUSD", "Litecoin to USD"),
            new CryptoPair("DOGEUSD", "Dogecoin to USD")
        };

    // Method to get the list of crypto pairs
    public IEnumerable<CryptoPair> GetAllCryptoPairs()
    {
      return AvailableCryptoPairs;
    }
  }
}
