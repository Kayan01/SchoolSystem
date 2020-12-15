using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.Zoom
{
    public class ZoomObj
    {
        public long id { get; set; }
        public string start_url { get; set; }
    }

    public class zoomReturnObject
    {
        public DateTime created_at { get; set; }
        public int duration { get; set; }
        public string host_id { get; set; }
        public long id { get; set; }
        public string join_url { get; set; }
        public string start_url { get; set; }
        public string status { get; set; }
        public string timezone { get; set; }
        public string topic { get; set; }
        public int type { get; set; }
        public string uuid { get; set; }
    }

    public class zoomSendingObject
    {
        public string topic { get; set; }
        public int type { get; set; }
        public string start_time { get; set; }
        public int duration { get; set; }
        public string schedule_for { get; set; }
        public string timezone { get; set; }
        public string password { get; set; }
        public string agenda { get; set; }
        public Recurrence recurrence { get; set; }
        public Settings settings { get; set; }
    }

    public class Recurrence
    {
        public int type { get; set; }
        public int repeat_interval { get; set; }
        public string weekly_days { get; set; }
        public int monthly_day { get; set; }
        public int monthly_week { get; set; }
        public int monthly_week_day { get; set; }
        public int end_times { get; set; }
        public string end_date_time { get; set; }
    }

    public class Settings
    {
        public bool host_video { get; set; }
        public bool participant_video { get; set; }
        public bool cn_meeting { get; set; }
        public bool in_meeting { get; set; }
        public bool join_before_host { get; set; }
        public bool mute_upon_entry { get; set; }
        public bool watermark { get; set; }
        public bool use_pmi { get; set; }
        public int approval_type { get; set; }
        public int registration_type { get; set; }
        public string audio { get; set; }
        public string auto_recording { get; set; }
        public bool meeting_authentication { get; set; }
        public bool registrants_email_notification { get; set; }
        public bool show_share_button { get; set; }
        public bool waiting_room { get; set; }
    }

}
