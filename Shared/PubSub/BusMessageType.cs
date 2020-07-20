using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.PubSub
{
    public enum BusMessageTypes
    {
        UNKNOWN,
        NEW_USER,
        EDIT_USER,
        EMAIL,
        NOTICE
    }
}
