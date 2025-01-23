using Microsoft.Extensions.Options;
using System.Net.Mail;


namespace API.Services.Email
{
    internal class ConfigEmail(IOptions<SecretEnv> SecretEnv) : IConfigEmail
    {
        public async Task configMail(string toEmail, string subject, string body)
        {
            MailMessage mail = new();

            SmtpClient smtpClient = new(SecretEnv.Value.SERVICE_SMTP);

            mail.From = new MailAddress(SecretEnv.Value.EMAIL_USER_SMTP);
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            smtpClient.Port = int.Parse(SecretEnv.Value.PORT_SMTP);
            smtpClient.Credentials = new System.Net.NetworkCredential(SecretEnv.Value.EMAIL_USER_SMTP, SecretEnv.Value.EMAIL_MDP_SMTP);
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


    internal interface IConfigEmail
    {
        Task configMail(string toEmail, string subject, string body);
    }
}