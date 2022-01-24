using Shared.Utils;
using System.Collections.Generic;
using System.ComponentModel;

namespace Shared.Permissions
{
    public enum Permission
    {
        [Description("USER CREATE")]
        USER_CREATE = 1001,
        [Description("USER READ")]
        USER_READ = 1002,
        [Description("USER UPDATE")]
        USER_UPDATE = 1003,
        [Description("USER DELETE")]
        USER_DELETE = 1004,

        [Description("TEACHER CREATE")]
        TEACHER_CREATE = 1011,
        [Description("TEACHER READ")]
        TEACHER_READ = 1012,
        [Description("TEACHER UPDATE")]
        TEACHER_UPDATE = 1013,
        [Description("TEACHER DELETE")]
        TEACHER_DELETE = 1014,

        [Description("STUDENT CREATE")]
        STUDENT_CREATE = 1021,
        [Description("STUDENT READ")]
        STUDENT_READ = 1022,
        [Description("STUDENT UPDATE")]
        STUDENT_UPDATE = 1023,
        [Description("STUDENT DELETE")]
        STUDENT_DELETE = 1024,

        [Description("ROLE CREATE")]
        ROLE_CREATE = 1031,
        [Description("ROLE READ")]
        ROLE_READ = 1032,
        [Description("ROLE UPDATE")]
        ROLE_UPDATE = 1033,
        [Description("ROLE DELETE")]
        ROLE_DELETE = 1034,

        [Description("FINANCE CREATE")]
        FINANCE_CREATE = 1041,
        [Description("FINANCE READ")]
        FINANCE_READ = 1042,
        [Description("FINANCE UPDATE")]
        FINANCE_UPDATE = 1043,
        [Description("FINANCE DELETE")]
        FINANCE_DELETE = 1044,
    }

    /*public static class PermisionProvider
    {
        public static Dictionary<string, IEnumerable<Permission>> GetSystemDefaultRoles()
        {

            var defaultRoles = new Dictionary<string, IEnumerable<Permission>>();
            defaultRoles.Add(FunctionHelper.PROCUREMENT_MANAGER, new Permission[]{
                Permission.USER_ADD,
                Permission.USER_DELETE,
                Permission.USER_LIST,
                Permission.USER_MENU,
            });

            return defaultRoles;


        }
    }*/
}