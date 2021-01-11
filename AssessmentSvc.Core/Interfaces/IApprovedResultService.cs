﻿using AssessmentSvc.Core.ViewModels.Result;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Interfaces
{
    public interface IApprovedResultService
    {
        Task<ResultModel<string>> SubmitStudentResult(UpdateApprovedStudentResultViewModel vm);
        Task<ResultModel<GetApprovedStudentResultViewModel>> GetStudentResultForApproval(GetStudentResultForApproval vm);
    }
}
