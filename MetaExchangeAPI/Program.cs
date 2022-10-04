using MetaExchange;
using Microsoft.Extensions.Options;

namespace MetaExchangeAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Action<Globals> globalsAction = (globals =>
            {
                string ordersFile = builder.Configuration["OrdersFile"];
                string ordersFilePath = Path.Combine(builder.Environment.ContentRootPath, ordersFile);
                var exchangesData = FetchJsonDataUtil.FetchExchangesFromFile(ordersFile);

                string clientBalancesFile = builder.Configuration["ClientBalancesFile"];
                string clientBalancesFilePath = Path.Combine(
                    builder.Environment.ContentRootPath, clientBalancesFile);
                var clientBalancesData = FetchJsonDataUtil.FetchClientBalancesFromFile(clientBalancesFile);

                globals.GlobalExchange = GlobalExchange.Create(exchangesData, clientBalancesData);
            });
            builder.Services.Configure<Globals>(globalsAction);
            builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<Globals>>().Value);

            // Globals.GlobalExchange = GlobalExchange.Create(exchangesData, clientBalancesData);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}