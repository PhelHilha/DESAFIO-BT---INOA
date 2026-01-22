namespace StockAlert.Services;

using System.Net.Http.Json;
using StockAlert.Interfaces;
using StockAlert.Models;

public class BrapiService : IStockService
{
    private readonly HttpClient _httpClient;
    private const string Token = "public"; 

    public BrapiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<StockResult> GetPriceAsync(string symbol)
    {
        try 
        {
            var response = await _httpClient.GetFromJsonAsync<BrapiResponse>($"/api/quote/{symbol}?token={Token}");

            if (response?.Results is null || response.Results.Count == 0)
                throw new InvalidOperationException($"Ativo {symbol} não encontrado na API.");

            var result = response.Results[0];
            
            // Log apenas para debug no console
            Console.WriteLine($"[API] {symbol}: R$ {result.RegularMarketPrice} | Ref: {result.RegularMarketTime}");
            
            return result; // Retorna o objeto completo com preço e hora
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[API ERROR] Falha ao obter cotação: {ex.Message}");
            throw; 
        }
    }
}