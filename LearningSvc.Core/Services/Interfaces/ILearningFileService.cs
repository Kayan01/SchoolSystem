using LearningSvc.Core.ViewModels.LearningFiles;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningSvc.Core.Services.Interfaces
{
    public interface ILearningFileService
    {
        Task<ResultModel<List<LearningFileListVM>>> GetAllFileByTeacher(long teacherId);
        Task<ResultModel<List<LearningFileListVM>>> GetAllFileByClass(long classId);
        Task<ResultModel<string>> UploadLearningFile(LearningFileUploadVM model);
        Task<ResultModel<string>> DeleteLearningFile(long fileId);
    }
}
