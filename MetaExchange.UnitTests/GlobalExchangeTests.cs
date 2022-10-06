using MetaExchange.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaExchange.UnitTests
{
    public class GlobalExchangeTests
    {
        [Test]
        public void Process_ReturnsFalse_WhenNotEnoughOrders()
        {
            // Arrange
            decimal eurBalance = 100000;
            decimal btcBalance = 5;

            // total bid orders amount = 0.5 < askExecAmount < client BTC balance = 5
            decimal askExecAmount = 1;

            var exchangeDto = ExchangeMockData.GetMockExchangeDto();
            var exchangesDto = new ExchangesDto
            {
                Exchanges = new List<ExchangeDto>() { exchangeDto }
            };
            var clientBalancesDto = ExchangeMockData
                .GetMockClientBalancesForExchangeDto(exchangeDto.Id, eurBalance, btcBalance);
            var globalExchange = GlobalExchange.Create(exchangesDto, clientBalancesDto);

            // Act
            Result<List<Order>> result = globalExchange.Process(ClientOrderTypes.SELL_BTC, askExecAmount);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.IsNull(result.Data);
            Assert.That(result.Message, Is.EqualTo(
                "Was not able to execute your order (not enough orders on all exchanges)"));
        }

        [Test]
        public void Process_ReturnsFalse_WhenNotEnoughFunds()
        {
            // Arrange
            decimal eurBalance = 100000;

            // client BTC balance = 0.49 < askExecAmount = total bid orders amount = 0.5
            decimal btcBalance = 0.49M;
            decimal askExecAmount = 0.5M;

            var exchangeDto = ExchangeMockData.GetMockExchangeDto();
            var exchangesDto = new ExchangesDto
            {
                Exchanges = new List<ExchangeDto>() { exchangeDto }
            };
            var clientBalancesDto = ExchangeMockData
                .GetMockClientBalancesForExchangeDto(exchangeDto.Id, eurBalance, btcBalance);
            var globalExchange = GlobalExchange.Create(exchangesDto, clientBalancesDto);

            // Act
            Result<List<Order>> result = globalExchange.Process(ClientOrderTypes.SELL_BTC, askExecAmount);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.IsNull(result.Data);
            Assert.That(result.Message, Is.EqualTo(
                "Was not able to execute your order (not enough orders on all exchanges)"));
        }
    }
}
