using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaExchange.Dto
{
    /// <summary>
    /// Client's balance for each asset type
    /// </summary>
    public class ClientBalanceDto
    {
        /// <summary>
        /// type of asset
        /// </summary>
        public AssetTypes AssetType { get; set; }

        /// <summary>
        /// balance
        /// </summary>
        public decimal Balance { get; set; }
    }
}
