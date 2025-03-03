using Microsoft.AspNetCore.Mvc;
using CryptoRealtimePrice.Services;
using CryptoRealtimePrice.Models;

namespace CryptoRealtimePrice.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CryptoPairsController : ControllerBase
  {
    private readonly TiingoRestClientService _TiingoRestClientService;
    private readonly CryptoPairService _cryptoService;
    private readonly ILogger<CryptoPairsController> _logger;
    public CryptoPairsController(CryptoPairService cryptoService, TiingoRestClientService TiingoRestClientService, ILogger<CryptoPairsController> logger)
    {
      _cryptoService = cryptoService;
      _TiingoRestClientService = TiingoRestClientService;
      _logger = logger;
    }

    /// <summary>
    /// Retrieves a list of available cryptocurrency pairs.
    /// </summary>
    /// <returns>Returns a list of supported cryptocurrency pairs.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<CryptoPair>), StatusCodes.Status200OK)]
    public IActionResult GetCryptoPairs()
    {
      _logger.LogInformation("Handling GET request for Crypto Pairs.");
      var cryptoPairs = _cryptoService.GetAllCryptoPairs();
      return Ok(cryptoPairs);
    }

    /// <summary>
    /// Retrieves the latest market price of a specified cryptocurrency pair.
    /// </summary>
    /// <param name="ticker">The cryptocurrency ticker (e.g., BTCUSD).</param>
    /// <returns>Returns the latest market price of the specified cryptocurrency.</returns>
    /// <response code="200">Returns the market price successfully.</response>
    /// <response code="400">Invalid ticker name.</response>
    /// <response code="403">API key invalid or rate limit exceeded.</response>
    /// <response code="404">Price data not found.</response>
    /// <response code="500">Unexpected server error.</response>
    [HttpGet("price/{ticker}")]
    [ProducesResponseType(typeof(CryptoPairPrice), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        var priceData = await _TiingoRestClientService.GetCryptoPriceAsync(ticker);
        if (priceData == null)
        {
          _logger.LogWarning($"Price data for {ticker} not found.");
          return NotFound($"Price data for {ticker} not found.");
        }

        var response = new CryptoPairPrice(ticker, priceData);
        return Ok(response);
      }
      catch (UnauthorizedAccessException ex)
      {
        _logger.LogError($"403 Forbidden: {ex.Message}");
        return StatusCode(403, "Forbidden: Access denied. API key might be invalid or rate limit exceeded.");
      }
      catch (HttpRequestException ex)
      {
        _logger.LogError($"Error fetching price data for {ticker}: {ex.Message}");
        return StatusCode(500, "Error fetching price data. Please try again later.");
      }
      catch (Exception ex)
      {
        _logger.LogError($"Unexpected error: {ex.Message}");
        return StatusCode(500, "An unexpected error occurred.");
      }
    }
  }
}