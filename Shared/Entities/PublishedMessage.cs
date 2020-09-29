using Shared.Entities.Auditing;
using Shared.Enums;
using Shared.PubSub;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Entities
{
    public class PublishedMessage: AuditedEntity<Guid>
    {
        public string Message { get; set; }
        public string Topic { get; set; }
        public BusMessageTypes MessageType { get; set; }
        public MessageStatus Status { get; set; }
    }
}