using Microsoft.AspNetCore.Mvc;
using CryptoRealtimePrice.Services;

namespace CryptoRealtimePrice.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CryptoPairsController : ControllerBase
  {
    private readonly TiingoClient _tiingoClient;
    private readonly CryptoPairService _cryptoService;
    private readonly ILogger<CryptoPairsController> _logger;
    public CryptoPairsController(CryptoPairService cryptoService, TiingoClient tiingoClient, ILogger<CryptoPairsController> logger)
    {
      _cryptoService = cryptoService;
      _tiingoClient = tiingoClient;
      _logger = logger;
    }

    // GET: api/Crypto
    [HttpGet]
    public IActionResult GetCryptoPairs()
    {
      _logger.LogInformation("Handling GET request for Crypto Pairs.");
      var cryptoPairs = _cryptoService.GetAllCryptoPairs();
      return Ok(cryptoPairs);
    }

    [HttpGet("price/{ticker}")]
    public async Task<IActionResult> GetCryptoPrice(string ticker)
    {
      try
      {
        var availablePairs = _cryptoService.GetAllCryptoPairs().Select(p => p.Ticker).ToList();
        if (!availablePairs.Contains(ticker.ToLower()))
        {
          _logger.LogWarning($"Ticker {ticker} is not in the supported list.");
          return BadRequest($"Invalid ticker: {ticker}. Please use one of the available tickers: {string.Join(", ", availablePairs)}");
        }

        var priceData = await _tiingoClient.GetCryptoPriceAsync(ticker);
        if (priceData == null)
        {
          _logger.LogWarning($"Price data for {ticker} not found.");
          return NotFound($"Price data for {ticker} not found.");
        }

        return Ok(new { Ticker = ticker, LastPrice = priceData });
      }
      catch (KeyNotFoundException ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}