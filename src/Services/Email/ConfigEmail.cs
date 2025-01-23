using System.Net.Mail;


namespace API.Services.Email
{
    internal class ConfigEmail(SecretEnv SecretEnv) : IConfigEmail
    {
        public async Task configMail(string toEmail, string subject, string body)
        {
            MailMessage mail = new();

            SmtpClient smtpClient = new(SecretEnv.SERVICE_SMTP);

            mail.From = new MailAddress(SecretEnv.EMAIL_USER_SMTP);
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            smtpClient.Port = int.Parse(SecretEnv.PORT_SMTP);
            smtpClient.Credentials = new System.Net.NetworkCredential(SecretEnv.EMAIL_USER_SMTP, SecretEnv.EMAIL_MDP_SMTP);
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