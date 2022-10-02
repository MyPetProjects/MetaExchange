using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaExchange.Dto
{
    /// <summary>
    /// THis class is needed specifically for reading data from json file
    /// </summary>
    public class ExchangesDto
    {
        public List<ExchangeDto> Exchanges { get; set; }

        /// <summary>
        /// convert data fetched from json file to a business object
        /// </summary>
        /// <returns></returns>
        public List<Exchange> ConvertToExchanges()
        {
            var exchanges = new List<Exchange>();

            Exchanges.ForEach(e => exchanges.Add(e.ConvertToExchange()));

            return exchanges;
        }
    }

}
