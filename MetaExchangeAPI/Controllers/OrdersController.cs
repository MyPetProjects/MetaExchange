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

        private readonly GlobalExchange _globalExchange;

        public OrdersController(ILogger<OrdersController> logger, GlobalExchange globalExchange)
        {
            _logger = logger;
            _globalExchange = globalExchange;
        }

        /// <summary>
        /// buy BTC
        /// </summary>
        /// <param name="amount">BTC amount</param>
        /// <returns>list of ask orders which should be executed</returns>
        /// <response code="200">Request successful</response>
        /// <response code="400">Order cannot be executed</response>
        [HttpPost("buy_btc")]
        public IActionResult BuyBtc(decimal amount)
        {
            Result<List<Order>> result= _globalExchange.Process(ClientOrderTypes.BUY_BTC, amount);

            if (result.Success)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        /// <summary>
        /// sell BTC
        /// </summary>
        /// <param name="amount">BTC amount</param>
        /// <returns>list of ask orders which should be executed</returns>
        /// <response code="200">Request successful</response>
        /// <response code="400">Order cannot be executed</response>
        [HttpPost("sell_btc")]
        public IActionResult SellBtc(decimal amount)
        {
            Result<List<Order>> result = _globalExchange.Process(ClientOrderTypes.SELL_BTC, amount);

            if (result.Success)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
    }
}
