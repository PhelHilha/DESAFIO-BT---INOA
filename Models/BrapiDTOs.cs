namespace StockAlert.Models;

using System.Text.Json.Serialization;

public class BrapiResponse
{
    [JsonPropertyName("results")]
    public List<StockResult>? Results { get; set; }
}

public class StockResult
{
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;

    [JsonPropertyName("regularMarketPrice")]
    public decimal RegularMarketPrice { get; set; }

    [JsonPropertyName("regularMarketTime")]
    public DateTime? RegularMarketTime { get; set; }
}