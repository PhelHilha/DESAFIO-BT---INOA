namespace StockAlert;

using StockAlert.Interfaces;
using StockAlert.Models;

public class StockMonitor
{
    private readonly IStockService _stockService;
    private readonly IEmailService _emailService;
    private readonly MonitorOptions _options;
    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1);

    public StockMonitor(IStockService stockService, IEmailService emailService, MonitorOptions options)
    {
        _stockService = stockService;
        _emailService = emailService;
        _options = options;
    }

    public async Task StartMonitoringAsync()
    {
        Console.WriteLine($"\n=== Iniciando Monitoramento: {_options.Asset} ===");
        Console.WriteLine($"Venda: > {_options.SellPrice} | Compra: < {_options.BuyPrice}");
        Console.WriteLine("Pressione Ctrl+C para encerrar.\n");

        while (true)
        {
            try
            {
                // Agora recebemos o objeto completo (result), não só o preço
                var result = await _stockService.GetPriceAsync(_options.Asset);
                
                decimal currentPrice = result.RegularMarketPrice;
                DateTime? dataHora = result.RegularMarketTime;

                if (currentPrice > _options.SellPrice)
                {
                    _emailService.SendEmail("Recomendação de Venda", 
                        $"O ativo {_options.Asset} subiu para R$ {currentPrice}.\n" +
                        $"Preço alvo de venda: {_options.SellPrice}.\n" +
                        $"Horário da cotação: {dataHora}");
                }
                else if (currentPrice < _options.BuyPrice)
                {
                    _emailService.SendEmail("Recomendação de Compra", 
                        $"O ativo {_options.Asset} caiu para R$ {currentPrice}.\n" +
                        $"Preço alvo de compra: {_options.BuyPrice}.\n" +
                        $"Horário da cotação: {dataHora}");
                }
            }
            catch
            {
                // Silencia exceções temporárias
            }

            await Task.Delay(_checkInterval);
        }
    }
}