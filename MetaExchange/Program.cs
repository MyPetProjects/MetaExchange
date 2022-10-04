using MetaExchange.Dto;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MetaExchange
{
    internal class Program
    {
        private const string DEFAULT_ORDERS_FILE = "Data\\order_books_data.json";

        private const string DEFAULT_CLIENT_BALANCES_FILE = "Data\\client_balances_data.json";

        private static ClientOrderTypes _clientOrderType;

        private static decimal _amount;

        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            if (!parseCmdLineArgs(args))
            {
                showHelp();
                Console.ReadKey();
                return;
            }

            string ordersFile = config["OrdersFile"] ?? DEFAULT_ORDERS_FILE;
            var exchangesData = FetchJsonDataUtil.FetchExchangesFromFile(ordersFile);

            string clientBalancesFile = config["ClientBalancesFile"] ?? DEFAULT_CLIENT_BALANCES_FILE;
            var clientBalancesData = FetchJsonDataUtil.FetchClientBalancesFromFile(clientBalancesFile);

            GlobalExchange globalExchange =
                GlobalExchange.Create(exchangesData, clientBalancesData);

            List<Order> resOrders = globalExchange.Process(_clientOrderType, _amount);

            Console.WriteLine("Orders which should be executed:");

            string resOrdersString = JsonSerializer.Serialize(resOrders, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            Console.WriteLine(resOrdersString);

            Console.ReadKey();
        }

        private static bool parseCmdLineArgs(string[] args)
        {
            if (args.Length != 2)
            {
                return false;
            }

            if (!Enum.TryParse(args[0], out _clientOrderType))
            {
                return false;
            }

            if (!decimal.TryParse(args[1], out _amount))
            {
                return false;
            }

            if (_amount <= 0)
            {
                Console.WriteLine("Order amount should be positive!");
                return false;
            }

            return true;
        }

        private static void showHelp()
        {
            Console.WriteLine("Incorrect command line arguments");
            Console.WriteLine("MetaExchange BUY_BTC|SELL_BTC [BTC amount]");
        }
    }
}