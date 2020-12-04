using LearningSvc.Core.ViewModels.AssignmentAnswer;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningSvc.Core.Interfaces
{
    public interface IAssignmentAnswerService
    {
        Task<ResultModel<List<AssignmentAnswerListVM>>> GetAllAnswer(long assignmentId);
        Task<ResultModel<AssignmentAnswerVM>> GetAssignmentAnswer(long answerId);
        Task<ResultModel<string>> UpdateScore(AssignmentAnswerUpdateScoreVM model);
        Task<ResultModel<string>> UpdateComment(AssignmentAnswerUpdateCommentVM model);
        Task<ResultModel<string>> AddAssignmentAnswer(AssignmentAnswerUploadVM assignmentAnswer, long currentUserId);
    }
}
