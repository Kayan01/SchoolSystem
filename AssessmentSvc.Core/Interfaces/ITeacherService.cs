using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Interfaces
{
    public interface ITeacherService
    {
        void AddOrUpdateTeacherFromBroadcast(TeacherSharedModel model);
    }
}
