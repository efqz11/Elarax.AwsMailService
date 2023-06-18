using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Elarax.AwsMailService
{
    public interface IEmailService
    {
        bool SendEmail(MailMessage mailMessage);
        Task<bool> SendEmailAsync(MailMessage mailMessage);

        Task<bool> SendEmailAsync(string to, string subejct, string body, bool isbodyHtml = true);

				Task<bool> SendEmailAsync(string to, string subejct, string body, bool isbodyHtml, IEmailServiceConfig emailServiceConfig);

        Task<MailMessage[]> SendMultipleEmailsAsync(params MailMessage[] mailMessages);

        MailMessage[] SendMultipleEmails(params MailMessage[] mailMessages);
    }
}
