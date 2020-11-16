using LearningSvc.Core.Enumerations;
using LearningSvc.Core.ViewModels.ClassWork;
using Shared.Pagination;
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
        Task<ResultModel<PaginatedModel<ClassWorkListVM>>> GetAllFileByTeacher(long currentUserId, QueryModel queryModel);
        Task<ResultModel<PaginatedModel<ClassWorkListVM>>> GetAllFileByClass(long classId, QueryModel queryModel);
        Task<ResultModel<string>> UploadLearningFile(ClassWorkUploadVM model, long currentUserId);
        Task<ResultModel<string>> DeleteClassNote(long id);
    }
}
