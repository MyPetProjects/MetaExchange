using MetaExchange;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace MetaExchangeAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<GlobalExchange>(provider =>
            {
                string ordersFile = builder.Configuration["OrdersFile"];
                string ordersFilePath = Path.Combine(builder.Environment.ContentRootPath, ordersFile);

                string clientBalancesFile = builder.Configuration["ClientBalancesFile"];
                string clientBalancesFilePath = Path.Combine(
                    builder.Environment.ContentRootPath, clientBalancesFile);

                return new ExchangeDataSource(
                    ordersFilePath, clientBalancesFilePath).GetGlobalExchange();
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Meta Exchange", Version = "v1" });
                c.IncludeXmlComments(Path.Combine(System.AppContext.BaseDirectory, "MetaExchangeAPI.xml"));
            });

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