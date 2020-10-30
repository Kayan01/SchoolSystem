using LearningSvc.Core.Enumerations;
using LearningSvc.Core.ViewModels.ClassWork;
using Shared.Utils;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningSvc.Core.Interfaces
{
    public interface IClassWorkService
    {
        Task<ResultModel<PaginatedList<ClassWorkListVM>>> GetAllFileByTeacher(long teacherId, int pagenumber, int pagesize);
        Task<ResultModel<PaginatedList<ClassWorkListVM>>> GetAllFileByClass(long classId, int pagenumber, int pagesize);
        Task<ResultModel<string>> UploadLearningFile(ClassWorkUploadVM model);
        Task<ResultModel<string>> DeleteLearningFile(long fileId);
    }
}
