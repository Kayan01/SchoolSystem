using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.PubSub
{
    public interface IBusMessage
    {
        int BusMessageType { get; set; }
    }
}
