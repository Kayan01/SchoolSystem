using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels
{
    public class CreateNotificationModel
    {
        public List<InAppNotificationModel> Notifications { get; set; } = new List<InAppNotificationModel>();
        public List<CreateEmailModel> Emails { get; set; } = new List<CreateEmailModel>();
    }
}
