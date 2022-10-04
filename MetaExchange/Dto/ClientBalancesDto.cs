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
    }
}
