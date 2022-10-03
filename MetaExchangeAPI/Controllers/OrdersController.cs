using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetaExchangeAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ILogger<OrdersController> logger)
        {
            _logger = logger;
        }

        [HttpPost("buy_btc")]
        public IActionResult BuyBtc(decimal amount)
        {
            return Problem("Endpoint not impelemnted");
        }

        [HttpPost("sell_btc")]
        public IActionResult SellBtc(decimal amount)
        {
            return Problem("Endpoint not impelemnted");
        }
    }
}
