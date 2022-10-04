using MetaExchange.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MetaExchange
{
    /// <summary>
    /// Util class for fetching data from json format
    /// </summary>
    public class FetchJsonDataUtil
    {
        private static T readFromFile<T>(string filePath)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());

            string text = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(text, options);
        }

        /// <summary>
        /// fetch exchanges data from a file
        /// </summary>
        /// <param name="ordersFileName">file path</param>
        /// <returns>dto object with fetched data</returns>
        public static ExchangesDto FetchExchangesFromFile(string ordersFileName)
        {
            return readFromFile<ExchangesDto>(ordersFileName);
        }

        /// <summary>
        /// fetch client balances from a file
        /// </summary>
        /// <param name="clientBalancesFileName">file path</param>
        /// <returns>dto object with fetched </returns>
        public static ClientBalancesDto FetchClientBalancesFromFile(string clientBalancesFileName)
        {
            return readFromFile<ClientBalancesDto>(clientBalancesFileName);
        }
    }
}
