using CryptoRealtimePrice.Services;

namespace CryptoRealtimePrice
{
  public static class ServiceRegistration
  {
    public static void AddApplicationServices(this IServiceCollection services)
    {
      // Register Crypto Pair and API Client Services
      services.AddSingleton<CryptoPairService>();
      services.AddScoped<TiingoClient>();

      // Register WebSocket Service
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
