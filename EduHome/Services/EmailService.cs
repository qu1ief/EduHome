using EduHome.ViewModels.Common;

namespace EduHome.Services;

using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;


public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendEmail(EmailVm vm)
    {
        SmtpClient smtpClient = new SmtpClient(_configuration["EmailSettings:Host"], int.Parse(_configuration["EmailSettings:Port"]))
        {
            Credentials = new NetworkCredential(_configuration["EmailSettings:Email"], _configuration["EmailSettings:Password"]),
            EnableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"]),
        };
        MailMessage mailMessage = new MailMessage()
        {
            Subject = vm.Subject,
            From = new MailAddress(_configuration["EmailSettings:Email"]),
            IsBodyHtml = bool.Parse(_configuration["EmailSettings:IsHtml"]),
        };
        mailMessage.To.Add(vm.To);

        mailMessage.Body = vm.Body;

        smtpClient.Send(mailMessage);
    }
}

public interface IEmailService
{
    void SendEmail(EmailVm vm);
}