using CryptoRealtimePrice.Models;

namespace CryptoRealtimePrice.Services
{
  public class CryptoPairService
  {
    // Hardcoded list of available crypto pairs
    private static readonly List<CryptoPair> AvailableCryptoPairs = new List<CryptoPair>
        {
            new CryptoPair("btcusd", "Bitcoin to USD"),
            new CryptoPair("ethusd", "Ethereum to USD"),
            new CryptoPair("xrpusd", "Ripple to USD"),
            new CryptoPair("ltcusd", "Litecoin to USD"),
            new CryptoPair("dogusdss", "Dogecoin to USD")
        };

    // Method to get the list of crypto pairs
    public IEnumerable<CryptoPair> GetAllCryptoPairs()
    {
      return AvailableCryptoPairs;
    }
  }
}
