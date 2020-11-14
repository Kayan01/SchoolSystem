using NotificationSvc.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;

namespace NotificationSvc.Core.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string[] emailAddresses, string emailTemplate, StringDictionary replacements);

        Task<IEnumerable<EmailVM>> GetEmails(string recipient, string subject);
    }
}
