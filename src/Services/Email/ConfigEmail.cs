using System.Net.Mail;
using Application.Interfaces.Services.Email;
using Domain.Utils;
using Microsoft.Extensions.Options;
namespace Application.Services.Email
{
    public class ConfigEmail(IOptions<SecretEnv> SecretEnv) : IConfigEmail
    {
        public async Task configMail(string toEmail, string subject, string body)
        {
            MailMessage mail = new();

            SmtpClient smtpClient = new(SecretEnv.Value.SERVICE);

            mail.From = new MailAddress(SecretEnv.Value.EMAIL_USER);
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            smtpClient.Port = int.Parse(SecretEnv.Value.PORT_SMTP);
            smtpClient.Credentials = new System.Net.NetworkCredential(SecretEnv.Value.EMAIL_USER, SecretEnv.Value.EMAIL_MDP);
            smtpClient.EnableSsl = true;

            try
            {
                await smtpClient.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email to {toEmail}: {ex.Message}");
            }
        }
    }
}