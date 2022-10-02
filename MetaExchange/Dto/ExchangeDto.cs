using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaExchange.Dto
{
    public class ExchangeDto
    {
        public string Id { get; set; }

        public DateTime AcqTime { get; set; }

        public List<OrderDto> Bids { get; set; }

        public List<OrderDto> Asks { get; set; }

        /// <summary>
        /// convert data fetched from json file to a business object
        /// </summary>
        /// <returns></returns>
        public Exchange ConvertToExchange()
        {
            var bids = new List<Order>();
            Bids.ForEach(b => bids.Add(b.ConvertToOrder(this.Id)));

            var asks = new List<Order>();
            Asks.ForEach(a => asks.Add(a.ConvertToOrder(this.Id)));

            return new Exchange
            {
                Id = this.Id,
                AcqTime = this.AcqTime,
                Bids = bids,
                Asks = asks
            };
        }
    }
}
