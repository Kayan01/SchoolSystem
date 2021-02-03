using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.Services.Interfaces
{
    public interface ISchoolClassService
    {
        void AddOrUpdateClassFromBroadcast(List<ClassSharedModel> model);
    }
}
