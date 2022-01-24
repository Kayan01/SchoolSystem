using Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Shared.ViewModels
{
    public class ClassAttendanceSharedModel
    {
        public long TenantId { get; set; }
        [DataType(DataType.Date)]
        public DateTime AttendanceDate { get; set; }
        public long StudentId { get; set; }
        public AttendanceState AttendanceStatus { get; set; }
        public string Remark { get; set; }
        public long ClassId { get; set; }
    }
}
