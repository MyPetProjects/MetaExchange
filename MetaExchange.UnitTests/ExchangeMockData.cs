using MetaExchange.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaExchange.UnitTests
{
    public class ExchangeMockData
    {
        public static ExchangeDto GetMockExchangeDto()
        {
            var exchangeId = Guid.NewGuid().ToString();

            // bid orders
            var bidOrder = new Order
            {
                Id = Guid.NewGuid(),
                ExchangeId = exchangeId,
                Time = DateTime.Now,
                Type = OrderTypes.BUY,
                Kind = "Limit",
                Amount = 0.5M,
                AmountLeft = 0.5M,
                AmountExecuted = 0,
                Price = 2100
            };

            var bidOrdersDto = new List<OrderDto>() { new OrderDto { Order = bidOrder } };

            // ask orders
            var askOrder = new Order
            {
                Id = Guid.NewGuid(),
                ExchangeId = exchangeId,
                Time = DateTime.Now,
                Type = OrderTypes.SELL,
                Kind = "Limit",
                Amount = 0.33M,
                AmountLeft = 0.33M,
                AmountExecuted = 0,
                Price = 2100
            };

            var askOrder2 = new Order
            {
                Id = Guid.NewGuid(),
                ExchangeId = exchangeId,
                Time = DateTime.Now,
                Type = OrderTypes.SELL,
                Kind = "Limit",
                Amount = 0.47M,
                AmountLeft = 0.47M,
                AmountExecuted = 0,
                Price = 2100
            };

            var askOrdersDto = new List<OrderDto>()
            {
                new OrderDto { Order = askOrder },
                new OrderDto { Order = askOrder2 }
            };

            return new ExchangeDto
            {
                Id = exchangeId,
                AcqTime = DateTime.Now,
                Bids = bidOrdersDto,
                Asks = askOrdersDto,
            };
        }

        public static List<ClientBalanceDto> GetMockClientBalancesDto(
            decimal eurBalance, decimal btcBalance)
        {
            var eurBalanceDto = new ClientBalanceDto
            {
                AssetType = AssetTypes.EUR,
                Balance = eurBalance
            };

            var btcBalanceDto = new ClientBalanceDto
            {
                AssetType = AssetTypes.BTC,
                Balance = btcBalance
            };

            return new List<ClientBalanceDto>() { eurBalanceDto, btcBalanceDto };
        }

        public static ClientBalancesDto GetMockClientBalancesForExchangeDto(
            string exchangeId, decimal eurBalance, decimal btcBalance)
        {
            return new ClientBalancesDto
            {
                ClientBalances = new List<ClientBalancesForExchangeDto>()
                {
                    new ClientBalancesForExchangeDto()
                    {
                        ExchangeId = exchangeId,
                        Balances = GetMockClientBalancesDto(eurBalance, btcBalance)
                    }
                }
            };
        }
    }
}
