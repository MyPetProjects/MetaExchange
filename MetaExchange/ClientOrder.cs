using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaExchange
{
    /// <summary>
    /// Order from a client
    /// </summary>
    public class ClientOrder
    {
        /// <summary>
        /// order type
        /// </summary>
        public ClientOrderTypes Type { get; private set; }

        /// <summary>
        /// amount to buy\sell
        /// </summary>
        public decimal Amount { get; private set; }
    }
}
