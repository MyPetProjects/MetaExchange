using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaExchange.UnitTests
{
    public class OrderTests
    {
        [Test]
        public void IsExecutable_ReturnsFalse_WhenExecAmountTooBig()
        {
            // Arrange
            decimal orderAmount = 0.1M;
            decimal execAmount = 0.99M;

            var order = new Order
            {
                Id = Guid.NewGuid(),
                ExchangeId = "123",
                Time = DateTime.Now,
                Type = OrderTypes.SELL,
                Kind = "Limit",
                Amount = orderAmount,
                AmountLeft = orderAmount,
                AmountExecuted = 0,
                Price = 2100
            };

            // Act
            Result<Order> result = order.IsExecutable(ClientOrderTypes.BUY_BTC, execAmount);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.That(result.Message, Is.EqualTo("Exec amount too big"));
        }

        [Test]
        public void IsExecutable_ReturnsTrue_WhenExecAmountEqualsToOrderAmount()
        {
            // Arrange
            decimal orderAmount = 0.1M;

            var order = new Order
            {
                Id = Guid.NewGuid(),
                ExchangeId = "123",
                Time = DateTime.Now,
                Type = OrderTypes.SELL,
                Kind = "Limit",
                Amount = orderAmount,
                AmountLeft = orderAmount,
                AmountExecuted = 0,
                Price = 2100
            };

            // Act
            Result<Order> result = order.IsExecutable(ClientOrderTypes.BUY_BTC, orderAmount);

            // Assert
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void IsExecutable_ReturnsTrue_WhenExecAmountLtOrderAmount()
        {
            // Arrange
            decimal orderAmount = 1.0M;
            decimal execAmount = 0.2M;

            var order = new Order
            {
                Id = Guid.NewGuid(),
                ExchangeId = "123",
                Time = DateTime.Now,
                Type = OrderTypes.SELL,
                Kind = "Limit",
                Amount = orderAmount,
                AmountLeft = orderAmount,
                AmountExecuted = 0,
                Price = 2100
            };

            // Act
            Result<Order> result = order.IsExecutable(ClientOrderTypes.BUY_BTC, execAmount);

            // Assert
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void IsExecutable_ReturnsFalse_WhenWrongOrderType()
        {
            // Arrange
            var wrongOrderType = OrderTypes.BUY;

            var order = new Order
            {
                Id = Guid.NewGuid(),
                ExchangeId = "123",
                Time = DateTime.Now,
                Type = wrongOrderType,
                Kind = "Limit",
                Amount = 0.1M,
                AmountLeft = 0.1M,
                AmountExecuted = 0,
                Price = 2100
            };

            // Act
            Result<Order> result = order.IsExecutable(ClientOrderTypes.BUY_BTC, 0.1M);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.That(result.Message, Is.EqualTo("Wrong order type"));
        }

        [Test]
        public void Execute_Fails_WhenExecAmountTooBig()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            decimal orderAmount = 0.1M;
            decimal execAmount = 0.99M;

            var order = new Order
            {
                Id = orderId,
                ExchangeId = "123",
                Time = DateTime.Now,
                Type = OrderTypes.SELL,
                Kind = "Limit",
                Amount = orderAmount,
                AmountLeft = orderAmount,
                AmountExecuted = 0,
                Price = 2100
            };

            // Act
            Result<Order> result = order.Execute(execAmount);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.That(result.Message, Is.EqualTo($"Not enough money left at order {orderId}"));
        }

        [Test]
        public void Execute_AmountsAreChanged()
        {
            // Arranges
            decimal orderAmount = 0.5M;
            decimal execAmount = 0.1M;

            var order = new Order
            {
                Id = Guid.NewGuid(),
                ExchangeId = "123",
                Time = DateTime.Now,
                Type = OrderTypes.SELL,
                Kind = "Limit",
                Amount = orderAmount,
                AmountLeft = orderAmount,
                AmountExecuted = 0,
                Price = 2100
            };

            // Act
            Result<Order> result = order.Execute(execAmount);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.That(order.Amount, Is.EqualTo(orderAmount),
                "We don't change order amount after execution");
            Assert.That(order.AmountLeft, Is.EqualTo(orderAmount - execAmount));
            Assert.That(order.AmountExecuted, Is.EqualTo(execAmount));
        }
    }
}
