using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.Interfaces
{
    public interface ISchoolClassSubjectService
    {
        void AddOrUpdateClassSubjectFromBroadcast(List<SchoolClassSubjectSharedModel> model);
    }
}
