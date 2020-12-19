using LearningSvc.Core.Enumerations;
using LearningSvc.Core.ViewModels.LearningFile;
using LearningSvc.Core.ViewModels.LessonNote;
using Shared.Pagination;
using Shared.Utils;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningSvc.Core.Interfaces
{
    public interface ILessonNoteService
    {
        Task<ResultModel<PaginatedModel<LessonNoteListVM>>> GetAllFileByTeacher(long currentUserId, QueryModel queryModel);
        Task<ResultModel<PaginatedModel<LessonNoteListVM>>> GetAllFileByClass(long classId, QueryModel queryModel);
        Task<ResultModel<PaginatedModel<StudentFileVM>>> GetAllFileByClassSubject(long classSubjectId, QueryModel queryModel);
        Task<ResultModel<LessonNoteVM>> LessonNoteDetail(long id);
        Task<ResultModel<string>> UploadLearningFile(LessonNoteUploadVM model, long currentUserId);
        Task<ResultModel<string>> DeleteLessonNote(long id);
    }
}
