using LearningSvc.Core.Enumerations;
using LearningSvc.Core.ViewModels.LessonNote;
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
        Task<ResultModel<PaginatedList<LessonNoteListVM>>> GetAllFileByTeacher(long teacherId, int pagenumber, int pagesize);
        Task<ResultModel<PaginatedList<LessonNoteListVM>>> GetAllFileByClass(long classId, int pagenumber, int pagesize);
        Task<ResultModel<string>> UploadLearningFile(LessonNoteUploadVM model);
        Task<ResultModel<string>> DeleteLessonNote(long id);
    }
}
