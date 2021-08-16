using AssessmentSvc.Core.Models;
using AssessmentSvc.Core.ViewModels.Result;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Interfaces
{
    public interface IResultSummaryService
    {
        Task<ResultModel<List<StudentResultSummaryVM>>> CalculateResultSummaries();
        Task<ResultModel<List<ResultSummary>>> GetResultSummaries(long? curSessionId = null, int? termSequenceNumber = null);
    }
}
