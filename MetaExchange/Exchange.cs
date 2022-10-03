using MetaExchange.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaExchange
{
    /// <summary>
    /// 
    /// </summary>
    public class Exchange
    {
        public string Id { get; set; }

        /// <summary>
        /// time when order book was acquired
        /// </summary>
        public DateTime AcqTime { get; set; }

        /// <summary>
        /// bid orders (people willing to buy BTC)
        /// </summary>
        public List<Order> Bids { get; set; }

        /// <summary>
        /// ask orders (people willing to sell BTC)
        /// </summary>
        public List<Order> Asks { get; set; }

        public Dictionary<AssetTypes, decimal> ClientBalances { get; set; }

        public void SetClientBalances(List<ClientBalanceDto> balances)
        {
            ClientBalances = new Dictionary<AssetTypes, decimal>();

            foreach (var balance in balances)
            {
                ClientBalances.Add(balance.AssetType, balance.Balance);
            }
        }

        /// <summary>
        /// check if an order can be executed
        /// </summary>
        /// <param name="clientOrderType">what a client wants to do</param>
        /// <param name="order">order which we are checking</param>
        /// <param name="execAmount">amount which we want to execute (orders can be executed partially)</param>
        /// <returns></returns>
        public bool IsOrderExecutable(ClientOrderTypes clientOrderType, Order order, decimal execAmount)
        {
            if (execAmount > order.AmountLeft) return false;

            if (clientOrderType == ClientOrderTypes.BUY_BTC)
            {
                if (order.Type != OrderTypes.SELL) return false;
                if (execAmount * order.Price > ClientBalances[AssetTypes.EUR]) return false;
            }

            if (clientOrderType == ClientOrderTypes.SELL_BTC)
            {
                if (order.Type != OrderTypes.BUY) return false;
                if (execAmount > ClientBalances[AssetTypes.BTC]) return false;
            }

            return true;
        }

        private void updateClientBalance(
            ClientOrderTypes clientOrderType, decimal execAmount, decimal orderPrice)
        {
            if (clientOrderType == ClientOrderTypes.BUY_BTC)
            {
                ClientBalances[AssetTypes.EUR] -= execAmount * orderPrice;
                ClientBalances[AssetTypes.BTC] += execAmount;
            }
            else if (clientOrderType == ClientOrderTypes.SELL_BTC)
            {
                ClientBalances[AssetTypes.EUR] += execAmount * orderPrice;
                ClientBalances[AssetTypes.BTC] -= execAmount;
            }
        }

        public void ExecuteOrder(Order order, ClientOrderTypes clientOrderType, decimal amount)
        {
            order.Execute(amount);
            updateClientBalance(clientOrderType, amount, order.Price);
        }
    }
}
