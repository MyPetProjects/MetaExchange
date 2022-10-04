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

        private GlobalExchange() { }

        private void setExchangesFromDto(ExchangesDto exchangesDto)
        {
            _exchanges = new List<Exchange>();
            exchangesDto.Exchanges.ForEach(e => _exchanges.Add(Exchange.Create(e)));
        }

        private void setClientBalancesForAllExchanges(ClientBalancesDto clientBalanceDto)
        {
            foreach (var clientBalance in clientBalanceDto.ClientBalances)
            {
                var exchangesFound = _exchanges.FindAll(e => e.Id == clientBalance.ExchangeId);

                if (exchangesFound.Count > 1)
                {
                    throw new Exception($"Exchange ID is not unique: {clientBalance.ExchangeId}");
                }

                if (exchangesFound.Count == 0)
                {
                    throw new Exception($"Exchange not found: {clientBalance.ExchangeId}");
                }

                var exchange = exchangesFound.First();

                exchange.SetClientBalances(clientBalance.Balances);
            }
        }

        /// <summary>
        /// create global exchange using exchanges\client balances data
        /// </summary>
        /// <param name="exchangesDto">exchanges data</param>
        /// <param name="clientBalancesDto">client balances data</param>
        /// <returns></returns>
        public static GlobalExchange Create(
            ExchangesDto exchangesDto, ClientBalancesDto clientBalancesDto)
        {
            GlobalExchange globalExchange = new();
            globalExchange.setExchangesFromDto(exchangesDto);
            globalExchange.setClientBalancesForAllExchanges(clientBalancesDto);

            return globalExchange;
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
