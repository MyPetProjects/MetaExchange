using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MetaExchange
{
    /// <summary>
    /// Possible exchange order types
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderTypes
    {
        BUY,

        SELL
    }
}
