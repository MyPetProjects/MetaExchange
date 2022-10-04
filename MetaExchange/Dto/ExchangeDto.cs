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
    }
}
