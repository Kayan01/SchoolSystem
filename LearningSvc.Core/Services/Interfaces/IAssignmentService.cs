using LearningSvc.Core.ViewModels.Assignment;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningSvc.Core.Services.Interfaces
{
    public interface IAssignmentService
    {
        Task<ResultModel<List<AssignmentGetVM>>> GetAssignmentsForTeacher(long teacherId);
        Task<ResultModel<List<AssignmentSubmissionListVM>>> GetAllSubmission(long assignmentId);
        Task<ResultModel<AssignmentSubmissionVM>> GetAssignmentSubmission(long submissionId);
        Task<ResultModel<string>> UpdateScore(AssignmentSubmissionUpdateScoreVM model);
        Task<ResultModel<string>> UpdateComment(AssignmentSubmissionUpdateCommentVM model);
        Task<ResultModel<string>> AddAssignment(AssignmentUploadVM assignment);
    }
}
