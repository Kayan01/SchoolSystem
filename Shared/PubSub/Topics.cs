using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.PubSub
{
    public static class Topics
    {
        public static string Class => nameof(Class).ToLower();
        public static string Class_List => nameof(Class_List).ToLower();
        public static string Notification => nameof(Notification).ToLower();
        public static string Staff => nameof(Staff).ToLower();
        public static string Student => nameof(Student).ToLower();
        public static string Teacher => nameof(Teacher).ToLower();
        public static string Admin => nameof(Admin).ToLower();

    }
}
