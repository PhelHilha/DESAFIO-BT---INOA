namespace StockAlert.Models;

public class EmailConfig
{
    public string SmtpServer { get; set; } = string.Empty;
    public int Port { get; set; }
    public string SenderEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string TargetEmail { get; set; } = string.Empty;
}