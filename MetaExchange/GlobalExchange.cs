using MetaExchange.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaExchange
{
    /// <summary>
    /// All exchanges and their orders assembled together
    /// </summary>
    public class GlobalExchange
    {
        private List<Exchange> _exchanges = new();

        public GlobalExchange(List<Exchange> exchanges)
        {
            _exchanges = new List<Exchange>(exchanges);
        }

        /// <summary>
        /// fill client balances for all exchanges
        /// </summary>
        /// <param name="clientBalancesDto">client balances data</param>
        public void SetClientBalances(ClientBalancesDto clientBalancesDto)
        {
            clientBalancesDto.SetClientBalancesForAllExchanges(_exchanges);
        }

        /// <summary>
        /// process client's order
        /// </summary>
        /// <param name="clientOrderType">what a client wants to do</param>
        /// <param name="amount">order amount</param>
        /// <returns>list of orders which need to be executed to fulfill client's request</returns>
        /// <exception cref="Exception"></exception>
        public List<Order> Process(ClientOrderTypes clientOrderType, decimal amount)
        {
            List<Order> resOrders = new();

            List<Order> orders = new();

            // get asks\bids from all exchanges sorted from the most to least profitable
            if (clientOrderType == ClientOrderTypes.BUY_BTC)
            {
                _exchanges.ForEach(e => orders.AddRange(e.Asks));
                orders = orders
                    .OrderBy(a => a.Price)
                    .ThenByDescending(a => a.AmountLeft)
                    .ToList();
            }
            else if (clientOrderType == ClientOrderTypes.SELL_BTC)
            {
                _exchanges.ForEach(e => orders.AddRange(e.Bids));
                orders = orders
                    .OrderByDescending(b => b.Price)
                    .ThenByDescending(b => b.AmountLeft)
                    .ToList();
            }

            decimal amountLeft = amount;

            foreach (var order in orders)
            {
                decimal execAmount = Math.Min(amountLeft, order.AmountLeft);
                var exchange = _exchanges.Find(e => e.Id == order.ExchangeId);

                if (!exchange.IsOrderExecutable(clientOrderType, order, execAmount))
                {
                    continue;
                }

                exchange.ExecuteOrder(order, clientOrderType, execAmount);
                amountLeft -= execAmount;
                resOrders.Add(order);

                if (amountLeft == 0) break;
            }

            if (amountLeft > 0)
            {
                // TODO: iterate through _resOrders and roll them all back
                throw new Exception("Was not able to execute your order (not enough orders on all exchanges)");
            }

            return resOrders;
        }
    }
}
