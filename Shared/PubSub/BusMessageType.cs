using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.PubSub
{
    public enum BusMessageTypes
    {
        UNKNOWN,
        EMAIL,
        NOTICE,
        TEACHER,
        TEACHER_UPDATE,
        TEACHER_DELETE,
        STAFF,
        STAFF_UPDATE,
        STAFF_DELETE,
        STUDENT,
        STUDENT_UPDATE,
        STUDENT_DELETE,
        CLASS,
        CLASS_UPDATE,
        CLASS_DELETE
    }
}
