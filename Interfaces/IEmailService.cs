namespace StockAlert.Interfaces;

public interface IEmailService
{
    void SendEmail(string subject, string body);
}