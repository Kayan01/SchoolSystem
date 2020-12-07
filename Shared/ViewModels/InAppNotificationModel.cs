using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels
{
    public class InAppNotificationModel
    {
        public InAppNotificationModel()
        {

        }

        public InAppNotificationModel(string description, string entityType, long? entityId, List<long> userIds)
        {
            Description = description;
            Entity = entityType;
            EntityId = entityId;
            UserIds = userIds;
        }

        public string Description { get; set; }
        public long? EntityId { get; set; }
        public string Entity { get; set; }
        public List<long> UserIds { get; set; } = new List<long>();
    }
}
