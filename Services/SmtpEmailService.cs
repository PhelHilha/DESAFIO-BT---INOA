namespace StockAlert.Services;

using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using StockAlert.Interfaces;
using StockAlert.Models;

public class SmtpEmailService : IEmailService
{
    private readonly EmailConfig _config;

    public SmtpEmailService(IOptions<EmailConfig> config)
    {
        _config = config.Value;
    }

    public void SendEmail(string subject, string body)
    {
        try
        {
            using var client = new SmtpClient(_config.SmtpServer, _config.Port)
            {
                Credentials = new NetworkCredential(_config.SenderEmail, _config.Password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_config.SenderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };
            
            mailMessage.To.Add(_config.TargetEmail);
            client.Send(mailMessage);

            Console.WriteLine($"[EMAIL] Enviado: {subject}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[EMAIL ERROR] Falha no envio: {ex.Message}");
        }
    }
}