using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaExchange.Dto
{
    public class OrderDto
    {
        public Order Order { get; set; }

        /// <summary>
        /// convert data fetched from json file to a business object
        /// </summary>
        /// <param name="exchangeId"></param>
        /// <returns></returns>
        public Order ConvertToOrder(string exchangeId = null)
        {
            return new Order
            {
                Id = Order.Id ?? Guid.NewGuid(),
                ExchangeId = exchangeId,
                Time = Order.Time,
                Type = Order.Type,
                Kind = Order.Kind,
                Amount = Order.Amount,
                AmountLeft = Order.Amount,
                AmountExecuted = 0,
                Price = Order.Price
            };
        }
    }
}
