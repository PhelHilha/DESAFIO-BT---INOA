using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StockAlert;
using StockAlert.Interfaces;
using StockAlert.Models;
using StockAlert.Services;

class Program
{
    static async Task Main(string[] args)
    {
        if (!ValidateArguments(args, out var asset, out var sellPrice, out var buyPrice))
            return;

        var host = CreateHostBuilder(asset, sellPrice, buyPrice).Build();
        
        var monitor = host.Services.GetRequiredService<StockMonitor>();
        await monitor.StartMonitoringAsync();
    }

    static IHostBuilder CreateHostBuilder(string asset, decimal sellPrice, decimal buyPrice)
    {
        return Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.Configure<EmailConfig>(context.Configuration.GetSection("EmailSettings"));

                services.AddHttpClient<IStockService, BrapiService>(client => 
                {
                    client.BaseAddress = new Uri("https://brapi.dev");
                });

                services.AddSingleton<IEmailService, SmtpEmailService>();
                services.AddSingleton(new MonitorOptions(asset, sellPrice, buyPrice));
                services.AddSingleton<StockMonitor>();
            });
    }

    static bool ValidateArguments(string[] args, out string asset, out decimal sellPrice, out decimal buyPrice)
    {
        asset = string.Empty;
        sellPrice = 0;
        buyPrice = 0;

        if (args.Length < 3)
        {
            Console.WriteLine("Uso: dotnet run <ATIVO> <PRECO_VENDA> <PRECO_COMPRA>");
            return false;
        }

        asset = args[0].ToUpper();
        sellPrice = ParsePrice(args[1]);
        buyPrice = ParsePrice(args[2]);

        if (sellPrice < 0 || buyPrice < 0)
        {
            Console.WriteLine("Erro: Preços inválidos.");
            return false;
        }

        return true;
    }

    static decimal ParsePrice(string input)
    {
        string normalized = input.Replace(",", ".");
        return decimal.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result) 
            ? result 
            : -1;
    }
}