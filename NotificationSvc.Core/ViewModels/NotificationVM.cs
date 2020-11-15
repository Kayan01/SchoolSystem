using NotificationSvc.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationSvc.Core.ViewModels
{
    public class NotificationVM
    {
        public long Id { get; set; }
        public string Description { get; set; }

        public static implicit operator NotificationVM(Notification model)
        {
            return model == null ? null : new NotificationVM
            {
                Id = model.Id,
                Description = model.Description
            };
        }
    }
}
