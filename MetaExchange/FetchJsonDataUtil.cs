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
    internal class FetchJsonDataUtil
    {
        private static T readFromFile<T>(string filePath)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());

            string text = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(text, options);
        }

        private static List<Exchange> fetchExchangesFromFile(string ordersFileName)
        {
            ExchangesDto exchangesDto = readFromFile<ExchangesDto>(ordersFileName);
            return exchangesDto.ConvertToExchanges();
        }

        private static ClientBalancesDto fetchClientBalancesFromFile(string clientBalancesFileName)
        {
            return readFromFile<ClientBalancesDto>(clientBalancesFileName);
        }

        /// <summary>
        /// fetch data from files and create a business object with all order books, client balances etc
        /// </summary>
        /// <param name="ordersFileName"></param>
        /// <param name="clientBalancesFileName"></param>
        /// <returns></returns>
        public static GlobalExchange CreateGlobalExchange(
            string ordersFileName, string clientBalancesFileName)
        {
            List<Exchange> exchanges = fetchExchangesFromFile(ordersFileName);
            var globalExchange = new GlobalExchange(exchanges);

            ClientBalancesDto clientBalancesDto =
                fetchClientBalancesFromFile(clientBalancesFileName);

            globalExchange.SetClientBalances(clientBalancesDto);

            return globalExchange;
        }
    }
}
