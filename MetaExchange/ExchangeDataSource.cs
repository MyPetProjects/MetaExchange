using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaExchange
{
    public class ExchangeDataSource
    {
        private const string DEFAULT_ORDERS_FILE = "Data\\order_books_data.json";

        private const string DEFAULT_CLIENT_BALANCES_FILE = "Data\\client_balances_data.json";

        private string _ordersFile;

        private string _clientBalancesFile;

        /// <summary>
        /// create ExchangeDataSource object
        /// </summary>
        /// <param name="ordersFile">path to a file with orders data</param>
        /// <param name="clientBalancesFile">path to a file with client balances</param>
        public ExchangeDataSource(string? ordersFile = null, string? clientBalancesFile = null)
        {
            _ordersFile = ordersFile ?? DEFAULT_ORDERS_FILE;
            _clientBalancesFile = clientBalancesFile ?? DEFAULT_CLIENT_BALANCES_FILE;
        }

        /// <summary>
        /// fetch data and create GlobalExchange object based on it
        /// </summary>
        /// <returns></returns>
        public GlobalExchange GetGlobalExchange()
        {
            var exchangesData = FetchJsonDataUtil.FetchExchangesFromFile(_ordersFile);
            var clientBalancesData =
                FetchJsonDataUtil.FetchClientBalancesFromFile(_clientBalancesFile);

            return GlobalExchange.Create(exchangesData, clientBalancesData);
        }
    }
}
