using Auth.Core.ViewModels.Setup;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Interfaces.Setup
{
    public interface ISchoolPropertyService
    {
        Task<ResultModel<SchoolPropertyVM>> GetSchoolProperty();
        Task<ResultModel<SchoolPropertyVM>> SetSchoolProperty(SchoolPropertyVM model);
    }
}
