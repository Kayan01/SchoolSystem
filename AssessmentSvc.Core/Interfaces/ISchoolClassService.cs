using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Interfaces
{
    public interface ISchoolClassService
    {
        Task AddOrUpdateClassFromBroadcast(List<ClassSharedModel> model);
    }
}
