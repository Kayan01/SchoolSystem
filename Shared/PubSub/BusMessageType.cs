﻿using System;
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
        CLASSSUBJECT,
        CLASSSUBJECT_UPDATE,
        CLASSSUBJECT_DELETE,
        ADMIN,
        ADMIN_DELETE,
        SUBJECT,
        SUBJECT_UPDATE,
        SUBJECT_DELETE,
        PARENT,
        PARENT_UPDATE,
        PARENT_DELETE,
        SESSION,
        SESSION_UPDATE,
        SESSION_DELETE,
        SCHOOL,
        SCHOOL_UPDATE,
        SCHOOL_DELETE,
        PROMOTION,
        PROMOTION_UPDATE,
        PROMOTION_DELETE,
        CLASSATTENDANCE,
        CLASSATTENDANCE_UPDATE,
        CLASSATTENDANCE_DELETE,
    }
}
