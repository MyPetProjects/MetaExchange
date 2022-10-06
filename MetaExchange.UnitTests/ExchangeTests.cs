using MetaExchange.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaExchange.UnitTests
{
    public class ExchangeTests
    {
        [Test]
        public void IsOrderExecutable_ReturnsFalse_WhenNotEnoughEUR()
        {
            // Arrange
            var exchange = Exchange.Create(ExchangeMockData.GetMockExchangeDto());
            exchange.SetClientBalances(ExchangeMockData.GetMockClientBalancesDto(100, 1));
            var askOrder = exchange.Asks.First();

            // Act
            Result<Exchange> result = exchange.IsOrderExecutable(ClientOrderTypes.BUY_BTC, askOrder, 0.33M);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.That(result.Message, Is.EqualTo("Client does not have enough funds"));
        }

        [Test]
        public void IsOrderExecutable_ReturnsFalse_WhenNotEnoughBTC()
        {
            // Arrange
            var exchange = Exchange.Create(ExchangeMockData.GetMockExchangeDto());
            exchange.SetClientBalances(ExchangeMockData.GetMockClientBalancesDto(100, 0.1M));
            var bidOrder = exchange.Bids.First();

            // Act
            Result<Exchange> result = exchange.IsOrderExecutable(ClientOrderTypes.SELL_BTC, bidOrder, 0.5M);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.That(result.Message, Is.EqualTo("Client does not have enough funds"));
        }
    }
}
