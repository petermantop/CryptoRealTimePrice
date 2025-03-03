using Microsoft.AspNetCore.SignalR;

namespace CryptoRealtimePrice.Hubs
{
  public class CryptoPriceHub : Hub
  {
    public async Task SubscribeToTicker(string ticker)
    {
      await Groups.AddToGroupAsync(Context.ConnectionId, ticker.ToLower());
      await Clients.Caller.SendAsync("SubscriptionConfirmed", $"Subscribed to {ticker}");
    }

    public async Task UnsubscribeFromTicker(string ticker)
    {
      await Groups.RemoveFromGroupAsync(Context.ConnectionId, ticker);
      await Clients.Caller.SendAsync("UnsubscriptionConfirmed", $"Unsubscribed from {ticker}");
    }
  }
}
