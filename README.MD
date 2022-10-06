# Meta Exchange

This solution consists of two projects: console application and API.

## Data files
There should be two files in json format: one with order books data and another with client balances. <br />
Paths to these files (absolute or relative to where MetaExchange.exe is located) should be in appsettings.json for a project. <br />
Console app fetches data from them at startup. Web app does it every time buy\sell BTC service is called <br />
(so we don't save state after order execution). <br />
Examples of such files are stored in folder \Data File Examples:
 - client_balances_data.json
 - order_books_data.json

## Console Application
1.) MetaExchange -> Set as Startup Project <br />
2.) Set what you want to do: buy or sell BTC and amount as command line arguments <br />
  `MetaExchange BUY_BTC|SELL_BTC [BTC amount]`

## API
1.) MetaExchangeAPI -> Set as Startup Project <br />
2.) API call examples are attached as Postman collection <br />
    &nbsp;&nbsp;&nbsp;\Postman Collections\MetaExchangeAPI.postman_collection.json
