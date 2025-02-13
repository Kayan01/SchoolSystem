﻿using NotificationSvc.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;

namespace NotificationSvc.Core.Services.Interfaces
{
    public interface IMailService
    {
        Task SendMailAsync(MailBase mail);
        Task SendMailAsync(MailBase mail, Dictionary<string, string> Replacements);
        void SendMail(MailBase mail);
        void SendMail(MailBase mail, Dictionary<string, string> Replacements);

        Task SendMailb(MailBase mail, string subject, string body);
    }
}
