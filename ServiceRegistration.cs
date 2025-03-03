using CryptoRealtimePrice.Services;

namespace CryptoRealtimePrice
{
  public static class ServiceRegistration
  {
    public static void AddApplicationServices(this IServiceCollection services)
    {
      services.AddSingleton<AppSettings>();

      // Register Crypto Pair and API Client Services
      services.AddSingleton<CryptoPairService>();
      services.AddSingleton<TiingoRestClientService>();
      services.AddSingleton<TiingoWebSocketService>();

      // Register SignalR Hub
      services.AddSignalR();

      // Register Controllers
      services.AddControllers();

      // Register Swagger (for API documentation)
      services.AddEndpointsApiExplorer();
      services.AddSwaggerGen();
    }
  }
}
