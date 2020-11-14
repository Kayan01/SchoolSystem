using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationSvc.Core.ViewModels
{
    public class EmailVM
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Address { get; set; }
        public bool Sent { get; set; }
        public string JsonReplacements { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

        public static implicit operator EmailVM(Email model)
        {
            return model == null ? null : new EmailVM
            {
                Subject = model.Subject,
                Address = model.Address,
                Body = model.Body,
                Sent = model.Sent,
                JsonReplacements = model.JsonReplacements,
                Created = model.Created,
                Modified = model.Modified
            };
        }
    }
}
