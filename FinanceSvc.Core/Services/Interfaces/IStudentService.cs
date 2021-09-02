using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.Services.Interfaces
{
    public interface IStudentService
    {
        void AddOrUpdateStudentFromBroadcast(List<StudentSharedModel> model);
    }
}
