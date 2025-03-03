using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using CryptoRealtimePrice.Hubs;
using CryptoRealtimePrice.Models;

namespace CryptoRealtimePrice.Services
{
  public class TiingoWebSocketService
  {
    private readonly IHubContext<CryptoPriceHub> _hubContext;
    private readonly CryptoPairService _cryptoService;
    private readonly ILogger<TiingoWebSocketService> _logger;
    private const string WebSocketUrl = "wss://api.tiingo.com/crypto";
    private const string ApiKey = "";
    private ClientWebSocket _webSocket;
    private readonly CancellationTokenSource _cts = new();

    public TiingoWebSocketService(IHubContext<CryptoPriceHub> hubContext, CryptoPairService cryptoService, ILogger<TiingoWebSocketService> logger)
    {
      _hubContext = hubContext;
      _cryptoService = cryptoService;
      _logger = logger;
      _webSocket = new ClientWebSocket();
    }

    public async Task StartWebSocketAsync()
    {
      try
      {
        await _webSocket.ConnectAsync(new Uri(WebSocketUrl), CancellationToken.None);
        _logger.LogInformation("Connected to Tiingo WebSocket.");

        var subscriptionMessage = JsonConvert.SerializeObject(new
        {
          eventName = "subscribe",
          authorization = ApiKey,
          eventData = new
          {
            thresholdLevel = "2"
          }
        });

        await SendMessageAsync(subscriptionMessage);
        await ReceiveMessagesAsync();
      }
      catch (Exception ex)
      {
        _logger.LogError($"WebSocket Error: {ex.Message}");
      }
    }

    private async Task SendMessageAsync(string message)
    {
      var bytes = Encoding.UTF8.GetBytes(message);
      var buffer = new ArraySegment<byte>(bytes);
      await _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
    }

    private async Task ReceiveMessagesAsync()
    {
      var buffer = new byte[4096];
      var availablePairs = _cryptoService.GetAllCryptoPairs().Select(p => p.Ticker.ToLower()).ToList(); // ✅ Get valid tickers

      while (_webSocket.State == WebSocketState.Open)
      {
        var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cts.Token);
        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);


        try
        {
          var baseMessage = JsonConvert.DeserializeObject<dynamic>(message);
          if (baseMessage == null)
          {
            _logger.LogWarning("Empty message received.");
            continue;
          }

          string messageType = baseMessage.messageType;

          if (messageType == "I")
          {
            var subscriptionResponse = JsonConvert.DeserializeObject<CryptoSubscriptionResponse>(message);
            _logger.LogInformation($"Subscription Confirmed: ID = {subscriptionResponse?.Data?.SubscriptionId}");
          }
          else if (messageType == "A")
          {
            if (baseMessage.data is Newtonsoft.Json.Linq.JArray dataArray)
            {
              string updateType = dataArray[0].ToString();
              string ticker = dataArray[1].ToString().ToLower(); // Convert to lowercase
              DateTime timestamp = DateTime.Parse(dataArray[2].ToString());

              // Check if the ticker is in the valid pairs list
              if (!availablePairs.Contains(ticker))
              {
                continue;
              }

              decimal marketPrice = 0;

              if (updateType == "T")
              {
                continue;
              }
              else if (updateType == "Q")
              {
                decimal bidPrice = Convert.ToDecimal(dataArray[5]);
                decimal askPrice = Convert.ToDecimal(dataArray[8]);
                marketPrice = (bidPrice + askPrice) / 2;
              }

              var priceUpdate = new { Ticker = ticker, MarketPrice = marketPrice, Timestamp = timestamp };
              _logger.LogInformation($"Updated Price: {marketPrice} for Pair: {ticker} at {timestamp}");
              await _hubContext.Clients.Group(ticker).SendAsync("ReceiveCryptoMarketPrice", priceUpdate);
            }
          }
        }
        catch (Exception ex)
        {
          _logger.LogError($"Deserialization Error: {ex.Message}");
        }
      }
    }

    public async Task StopWebSocketAsync()
    {
      _cts.Cancel();
      await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
      _logger.LogInformation("WebSocket Disconnected.");
    }
  }
}
