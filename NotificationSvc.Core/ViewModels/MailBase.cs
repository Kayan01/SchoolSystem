﻿using System.Collections.Generic;
using System.Net.Mail;

namespace NotificationSvc.Core.ViewModels
{
    public abstract class MailBase
    {
        public bool BodyIsFile { get; set; }
        public string Body { get; set; }
        public string BodyPath { get; set; }
        public string Subject { get; set; }
        public string Sender { get; set; }
        public string SenderDisplayName { get; set; }
        public bool IsBodyHtml { get; set; }
        public ICollection<string> To { get; set; }
        public ICollection<string> Bcc { get; set; }
        public ICollection<string> CC { get; set; }
        public ICollection<Attachment> Attachments { get; set; }
        public string EmailPassword { get; set; }
    }

    public sealed class Mail : MailBase
    {
        private Mail()
        {
            IsBodyHtml = true;
            To = new List<string>();
            CC = new List<string>();
            Bcc = new List<string>();
            Attachments = new List<Attachment>();
        }

        public Mail(string sender, string subject, string emailPassword, params string[] to)
            : this()
        {
            Sender = sender;
            Subject = subject;
            EmailPassword = emailPassword;

            foreach (var rec in to)
                To.Add(rec);
        }
    }
}
