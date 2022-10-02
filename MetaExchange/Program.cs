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

        private static List<Order> _resOrders = new();

        static void Main(string[] args)
        {
            if (!parseCmdLineArgs(args))
            {
                showHelp();
            }

            fillOrdersInfo();

            process();

            Console.WriteLine("Orders which should be executed:");

            string resOrdersString = JsonSerializer.Serialize(_resOrders, new JsonSerializerOptions
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

        private static void process()
        {
            List<Order> orders = new();
            // get asks\bids from all exchanges sorted from the most to least profitable
            if (_clientOrderType == ClientOrderTypes.BUY_BTC)
            {
                _exchanges.ForEach(e => orders.AddRange(e.Asks));
                orders = orders.OrderBy(a => a.Price).ToList();
            }
            else if (_clientOrderType == ClientOrderTypes.SELL_BTC)
            {
                _exchanges.ForEach(e => orders.AddRange(e.Bids));
                orders = orders.OrderByDescending(b => b.Price).ToList();
            }

            if (_resOrders == null) _resOrders = new List<Order>();
            decimal amountLeft = _amount;

            foreach(var order in orders)
            {
                decimal execAmount = Math.Min(amountLeft, order.AmountLeft);
                var balances = _exchanges.Find(e => e.Id == order.ExchangeId).ClientBalances;

                // TODO: move this logic to exchanges\orders
                if (_clientOrderType == ClientOrderTypes.BUY_BTC)
                {
                    if (execAmount * order.Price > balances[AssetTypes.EUR])
                    {
                        continue;
                    }

                    balances[AssetTypes.EUR] -= execAmount * order.Price;
                    balances[AssetTypes.BTC] += execAmount;
                }
                else if (_clientOrderType == ClientOrderTypes.SELL_BTC)
                {
                    if (execAmount > balances[AssetTypes.BTC])
                    {
                        continue;
                    }

                    balances[AssetTypes.EUR] += execAmount * order.Price;
                    balances[AssetTypes.BTC] -= execAmount;
                }

                amountLeft -= execAmount;
                order.AmountLeft -= execAmount;
                order.AmountExecuted += execAmount;
                _resOrders.Add(order);

                if (amountLeft == 0) break;
            }

            if (amountLeft > 0)
            {
                // TODO: iterate through _resOrders and roll them all back
                throw new Exception("Was not able to execute your order (not enough orders on all exchanges)");
            }
        }
    }
}