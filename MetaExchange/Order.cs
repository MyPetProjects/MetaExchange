using MetaExchange.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MetaExchange
{
    public class Order
    {
        public Guid? Id { get; set; }

        /// <summary>
        /// ID of an exchange to which this order belongs
        /// </summary>
        public string ExchangeId { get; set; }

        /// <summary>
        /// when the order was created
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// order type
        /// </summary>
        public OrderTypes Type { get; set; }

        public string Kind { get; set; }

        public decimal Amount { get; set; }

        [JsonIgnore]
        public decimal AmountLeft { get; set; }

        public decimal AmountExecuted { get; set; }

        public decimal Price { get; set; }

        /// <summary>
        /// create order using raw order data and assign it to an exchange
        /// </summary>
        /// <param name="orderDto">order data</param>
        /// <param name="exchangeId">exchange ID</param>
        /// <returns></returns>
        public static Order Create(OrderDto orderDto, string exchangeId = null)
        {
            Order order = orderDto.Order;

            return new Order
            {
                Id = order.Id ?? Guid.NewGuid(),
                ExchangeId = exchangeId,
                Time = order.Time,
                Type = order.Type,
                Kind = order.Kind,
                Amount = order.Amount,
                AmountLeft = order.Amount,
                AmountExecuted = 0,
                Price = order.Price
            };
        }

        /// <summary>
        /// check if this order could be executed
        /// </summary>
        /// <param name="clientOrderType">what client wants to do</param>
        /// <param name="execAmount">amount of BTC a client wants to buy\sell</param>
        /// <returns></returns>
        public Result<Order> IsExecutable(ClientOrderTypes clientOrderType, decimal execAmount)
        {
            if (execAmount > AmountLeft)
            {
                return Result<Order>.Fail("Exec amount too big");
            }

            if ((clientOrderType == ClientOrderTypes.BUY_BTC && Type != OrderTypes.SELL) ||
                (clientOrderType == ClientOrderTypes.SELL_BTC && Type != OrderTypes.BUY))
            {
                return Result<Order>.Fail("Wrong order type");
            }

            return Result<Order>.Ok();
        }

        /// <summary>
        /// execute order
        /// </summary>
        /// <param name="amount">amount to execute (order can be executed partially)</param>
        /// <exception cref="Exception"></exception>
        public Result<Order> Execute(decimal amount)
        {
            if (amount > AmountLeft)
            {
                return Result<Order>.Fail($"Not enough money left at order {Id}");
            }

            AmountLeft -= amount;
            AmountExecuted += amount;

            return Result<Order>.Ok();
        }
    }
}
