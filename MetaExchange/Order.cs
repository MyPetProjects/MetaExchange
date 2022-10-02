using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaExchange
{
    public class Order
    {
        public Guid? Id { get; set; }

        /// <summary>
        /// ID of an exchange to which this order belongs
        /// </summary>
        public string ExchangeId { get; set; }

        /// <summary>
        /// when the order was created
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// order type
        /// </summary>
        public OrderTypes Type { get; set; }

        public string Kind { get; set; }

        public decimal Amount { get; set; }

        public decimal AmountLeft { get; set; }

        public decimal Price { get; set; }
    }
}
