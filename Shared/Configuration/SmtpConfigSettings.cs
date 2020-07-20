using MailKit.Net.Smtp;
using MailKit.Security;
using System.Threading.Tasks;
namespace Shared.Configuration
{
    public class SmtpConfig
    {
        public bool UseSSl { get; set; }
        public int Port { get; set; }
        public string Server { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public bool UseDefaultCredentials { get; set; }

        public async Task<SmtpClient> BuildSMTPClient()
        {
            var client = new SmtpClient
            {
                ServerCertificateValidationCallback = (s, c, h, e) => true
            };

            await client.ConnectAsync(Server, Port, SecureSocketOptions.StartTls);

            if (!UseDefaultCredentials)
                await client.AuthenticateAsync(UserName, Password);

            return client;
            //await client.SendAsync(mimeMessage);
            //await client.DisconnectAsync(true);
        }
    }
}