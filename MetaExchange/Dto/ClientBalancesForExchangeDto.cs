using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaExchange.Dto
{
    /// <summary>
    /// Client balances of all asset types for a given exchange
    /// </summary>
    public class ClientBalancesForExchangeDto
    {
        public string ExchangeId { get; set; }

        public List<ClientBalanceDto> Balances { get; set; }
    }
}
