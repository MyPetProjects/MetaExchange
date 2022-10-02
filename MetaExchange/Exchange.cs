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
    }
}
