using LearningSvc.Core.ViewModels.Assignment;
using Shared.Pagination;
using Shared.Utils;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningSvc.Core.Interfaces
{
    public interface IAssignmentService
    {
        Task<ResultModel<PaginatedModel<AssignmentGetVM>>> GetAssignmentsForTeacher(long currentUserId, QueryModel queryModel);
        Task<ResultModel<PaginatedModel<AssignmentGetVM>>> GetAssignmentsForClass(long classId, QueryModel queryModel);
        Task<ResultModel<List<AssignmentSubmissionListVM>>> GetAllSubmission(long assignmentId);
        Task<ResultModel<AssignmentSubmissionVM>> GetAssignmentSubmission(long submissionId);
        Task<ResultModel<string>> UpdateScore(AssignmentSubmissionUpdateScoreVM model);
        Task<ResultModel<string>> UpdateComment(AssignmentSubmissionUpdateCommentVM model);
        Task<ResultModel<string>> AddAssignment(AssignmentUploadVM assignment, long currentUserId);
    }
}
