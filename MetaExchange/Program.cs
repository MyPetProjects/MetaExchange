using MetaExchange.Dto;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MetaExchange
{
    internal class Program
    {
        private static ClientOrderTypes _clientOrderType;

        private static decimal _amount;

        private static List<Exchange> _exchanges = new();

        static void Main(string[] args)
        {
            if (!parseCmdLineArgs(args))
            {
                showHelp();
            }

            fillOrdersInfo();

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

            return true;
        }

        private static void showHelp()
        {
            Console.WriteLine("Incorrect command line arguments");
            Console.WriteLine("MetaExchange BUY_BTC|SELL_BTC [BTC amount]");
        }

        private static void fillOrdersInfo(string ordersFileName = "order_books_data.json",
            string clientBalancesFileName = "client_balances_data.json")
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            string text;

            text = File.ReadAllText(ordersFileName);
            ExchangesDto exchangesDto = JsonSerializer.Deserialize<ExchangesDto>(text, options);
            _exchanges = exchangesDto.ConvertToExchanges();

            text = File.ReadAllText(clientBalancesFileName);
            ClientBalancesDto clientBalancesDto = JsonSerializer.Deserialize<ClientBalancesDto>(text, options);

            clientBalancesDto.SetClientBalancesForAllExchanges(_exchanges);
        }

        private static void Process()
        {
            // _clientOrderType, _amount, _exchanges => resultOrders
            // Console.WriteLine(resultOrders);
        }
    }
}