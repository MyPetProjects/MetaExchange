using MetaExchange;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetaExchangeAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;

        private readonly Globals _globals;

        public OrdersController(ILogger<OrdersController> logger, Globals globals)
        {
            _logger = logger;
            _globals = globals;
        }

        [HttpPost("buy_btc")]
        public IActionResult BuyBtc(decimal amount)
        {
            List<Order> resOrders = _globals.GlobalExchange.Process(ClientOrderTypes.BUY_BTC, amount);

            return Ok(resOrders);
        }

        [HttpPost("sell_btc")]
        public IActionResult SellBtc(decimal amount)
        {
            List<Order> resOrders = _globals.GlobalExchange.Process(ClientOrderTypes.SELL_BTC, amount);

            return Ok(resOrders);
        }
    }
}
