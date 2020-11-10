using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.ClassSession
{
    public class ClassSessionVM
    {
        public long Id { get; internal set; }
        public long ClassId { get; internal set; }
        public long TeacherId { get; internal set; }
        public long SubjectId { get; internal set; }
        public bool EnableVirtual { get; internal set; }
        public DateTime? DateTimeOfClass { get; internal set; }
        public string ZoomRoomId { get; set; }
    }
}
