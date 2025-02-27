using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CryptoRealtimePrice.Services;

namespace CryptoRealtimePrice.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CryptoPairsController : ControllerBase
  {
    private readonly CryptoPairService _cryptoService;
    private readonly ILogger<CryptoPairsController> _logger;
    public CryptoPairsController(CryptoPairService cryptoService, ILogger<CryptoPairsController> logger)
    {
      _cryptoService = cryptoService;
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
  }
}