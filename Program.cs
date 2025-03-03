using CryptoRealtimePrice;
using CryptoRealtimePrice.Services;
using CryptoRealtimePrice.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Configure Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventSourceLogger();
builder.Logging.AddFilter("Microsoft", LogLevel.Warning)
	.AddFilter("System", LogLevel.Warning)
	.AddFilter("CryptoRealtimePrice", LogLevel.Debug);

// Register all application services via `ServiceRegistration`
builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure Swagger (API Documentation)
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
app.MapHub<CryptoPriceHub>("/cryptoPriceHub");

// Start WebSocket service
var webSocketService = app.Services.GetRequiredService<TiingoWebSocketService>();
_ = webSocketService.StartWebSocketAsync();

app.Run();