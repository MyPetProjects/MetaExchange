using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaExchange.Dto
{
    /// <summary>
    /// Client balances of all asset types on all exchanges
    /// </summary>
    public class ClientBalancesDto
    {
        public List<ClientBalancesForExchangeDto> ClientBalances { get; set; }

        /// <summary>
        /// fill exchanges client balance properties with data fetched from file
        /// </summary>
        /// <param name="exchanges">list of exchanges</param>
        /// <exception cref="Exception"></exception>
        public void SetClientBalancesForAllExchanges(List<Exchange> exchanges)
        {
            foreach(var clientBalance in ClientBalances)
            {
                var exchangesFound = exchanges.FindAll(e => e.Id == clientBalance.ExchangeId);

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
    }
}
