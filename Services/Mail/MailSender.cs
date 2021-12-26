using System.Net;
using System.Net.Mail;
using System.Text;
using BlazorServerTest.Data.Mail;
using Microsoft.Extensions.Options;

namespace BlazorServerTest.Services.Mail; 

public class MailSender : IMailSender  {

    private readonly ILogger<MailSender> _logger;
    private readonly IOptionsMonitor<MailSenderOptions> _options;

    public MailSender(ILogger<MailSender> logger) {
        _logger = logger;
    }

    public MailSender(ILogger<MailSender> logger, IOptionsMonitor<MailSenderOptions> options) {
        _logger = logger;
        _options = options;
    }
    
    public async Task SendEmailAsync(string?[] recipients, string subject, string body) {
        var options = _options.CurrentValue;

        var msg = new MailMessage {
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
            From = new MailAddress(options.AddressFrom),
            BodyEncoding = Encoding.UTF8
        };

        foreach ( var recipient in recipients ) {
            msg.To.Add(recipient);
        }

        // Build SMTP Server
        using var client = new SmtpClient {
            Host = options.SmtpHostAddress,
            Port = options.Port,
            EnableSsl = true,
            Credentials = new NetworkCredential(options.AddressFrom, options.Password)
        };
        
        await client.SendMailAsync(msg);
        
        _logger.LogDebug("Sent email to {Address}", options.AddressFrom);
    }
}