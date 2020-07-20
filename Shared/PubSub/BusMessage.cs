using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.PubSub
{
    [ProtoContract]
    public class BusMessage
    {
        [ProtoMember(1)]
        public int BusMessageType { get; set; }
        [ProtoMember(2)]
        public string Data { get; set; }

    }
}
