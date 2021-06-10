using AssessmentSvc.Core.ViewModels.Result;
using Microsoft.AspNetCore.Http;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Interfaces
{
    public interface IResultService
    {
        Task<ResultModel<string>> InsertBehaviouralResult(AddBehaviourResultVM model); 
        Task<ResultModel<GetBehaviourResultVM>> GetBehaviouralResult(GetBehaviourResultQueryVm model);
        Task<List<KeyValuePair<long, GetBehaviourResultVM>>> GetBehaviouralResults(GetBehaviourResultQueryVm model);
        Task<ResultModel<byte[]>> GenerateResultUploadExcel(long SchoolClassId);
        Task<ResultModel<ResultUploadFormData>> FetchResultUploadFormData(long SchoolClassId);
        Task<ResultModel<string>> ProcessResultFromExcel(ResultFileUploadVM vM);
        Task<ResultModel<string>> ProcessResult(ResultUploadVM models);
        Task<ResultModel<List<ResultBroadSheet>>> GetClassBroadSheet(long classId);
        Task<ResultModel<IndividualBroadSheet>> GetStudentResultSheet(long classId, long studentId);
    }
}
