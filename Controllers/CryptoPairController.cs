using Microsoft.AspNetCore.Mvc;
using CryptoRealtimePrice.Services;

namespace CryptoRealtimePrice.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CryptoPairsController : ControllerBase
  {
    private readonly CryptoPairService _cryptoService;

    public CryptoPairsController(CryptoPairService cryptoService)
    {
      _cryptoService = cryptoService;
    }

    // GET: api/Crypto
    [HttpGet]
    public IActionResult GetCryptoPairs()
    {
      var cryptoPairs = _cryptoService.GetAllCryptoPairs();
      return Ok(cryptoPairs);
    }
  }
}