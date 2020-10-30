using LearningSvc.Core.Enumerations;
using LearningSvc.Core.ViewModels.Media;
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
        Task<ResultModel<PaginatedList<MediaListVM>>> GetAllFileByTeacher(long teacherId, int pagenumber, int pagesize);
        Task<ResultModel<PaginatedList<MediaListVM>>> GetAllFileByClass(long classId, int pagenumber, int pagesize);
        Task<ResultModel<string>> UploadLearningFile(MediaUploadVM model);
        Task<ResultModel<string>> DeleteLearningFile(long fileId);
    }
}
