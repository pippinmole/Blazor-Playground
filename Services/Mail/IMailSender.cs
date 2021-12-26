namespace BlazorServerTest.Services.Mail; 

public interface IMailSender {
    Task SendEmailAsync(string?[] recipients, string subject, string body);
}