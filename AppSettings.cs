public class AppSettings
{
  public string TiingoApiKey { get; }

  public AppSettings(IConfiguration configuration)
  {
    TiingoApiKey = Environment.GetEnvironmentVariable("TIINGO_API_KEY")
                   ?? configuration["ApiSettings:TiingoApiKey"]
                   ?? throw new InvalidOperationException("API key is missing. Set the TIINGO_API_KEY environment variable or define it in appsettings.json.");
  }
}
