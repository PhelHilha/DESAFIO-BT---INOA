namespace StockAlert.Interfaces;
using StockAlert.Models;
public interface IStockService
{
    Task<StockResult> GetPriceAsync(string symbol);
}