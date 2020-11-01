using LearningSvc.Core.ViewModels.Assignment;
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
        Task<ResultModel<PaginatedList<AssignmentGetVM>>> GetAssignmentsForTeacher(long teacherId, int pagenumber, int pagesize);
        Task<ResultModel<PaginatedList<AssignmentGetVM>>> GetAssignmentsForClass(long classId, int pagenumber, int pagesize);
        Task<ResultModel<List<AssignmentSubmissionListVM>>> GetAllSubmission(long assignmentId);
        Task<ResultModel<AssignmentSubmissionVM>> GetAssignmentSubmission(long submissionId);
        Task<ResultModel<string>> UpdateScore(AssignmentSubmissionUpdateScoreVM model);
        Task<ResultModel<string>> UpdateComment(AssignmentSubmissionUpdateCommentVM model);
        Task<ResultModel<string>> AddAssignment(AssignmentUploadVM assignment);
    }
}
