using FinanceSvc.Core.Models;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FinanceSvc.Core.Interfaces
{
    public interface ISchoolService
    {
        void AddOrUpdateSchoolFromBroadcast(SchoolSharedModel model);
        Task<School> GetSchool(long id);
    }
}
