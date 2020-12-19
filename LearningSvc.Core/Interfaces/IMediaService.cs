using LearningSvc.Core.Enumerations;
using LearningSvc.Core.ViewModels.ClassWork;
using LearningSvc.Core.ViewModels.LearningFile;
using LearningSvc.Core.ViewModels.Media;
using Shared.Pagination;
using Shared.Utils;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningSvc.Core.Interfaces
{
    public interface IMediaService
    {
        Task<ResultModel<PaginatedModel<MediaListVM>>> GetAllFileByTeacher(long currentUserId, QueryModel queryModel);
        Task<ResultModel<PaginatedModel<MediaListVM>>> GetAllFileByClass(long classId, QueryModel queryModel);
        Task<ResultModel<PaginatedModel<StudentFileVM>>> GetAllFileByClassSubject(long classSubjectId, QueryModel queryModel);
        Task<ResultModel<MediaVM>> MediaDetail(long id);
        Task<ResultModel<string>> UploadLearningFile(MediaUploadVM model, long currentUserId);
        Task<ResultModel<string>> DeleteMedia(long id);
    }
}
