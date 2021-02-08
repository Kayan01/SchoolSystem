using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.PubSub
{
    public enum BusMessageTypes
    {
        UNKNOWN,
        EMAIL,
        NOTIFICATION,
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
        CLASS_DELETE,
        ADMIN,
        ADMIN_DELETE,
        SUBJECT,
        SUBJECT_UPDATE,
        SUBJECT_DELETE,
        PARENT,
        PARENT_UPDATE,
        PARENT_DELETE,
    }
}
