using CryptoRealtimePrice.Services;

namespace CryptoRealtimePrice
{
  public static class ServiceRegistration
  {
    public static void AddApplicationServices(this IServiceCollection services)
    {
      services.AddScoped<CryptoPairService>();
    }
  }
}
