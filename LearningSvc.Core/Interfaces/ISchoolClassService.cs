﻿using LearningSvc.Core.ViewModels.SchoolClass;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningSvc.Core.Interfaces
{
    public interface ISchoolClassService
    {
        Task<ResultModel<List<SchoolClassVM>>> GetAllSchoolClass();
        Task<ResultModel<SchoolClassVM>> AddSchoolClass(SchoolClassVM model);
        Task AddOrUpdateClassFromBroadcast(ClassSharedModel model);
    }
}
