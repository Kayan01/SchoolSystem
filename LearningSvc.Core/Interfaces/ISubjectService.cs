using LearningSvc.Core.ViewModels.Subject;
using Shared.Pagination;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningSvc.Core.Interfaces
{
    public interface ISubjectService
    {
        Task<ResultModel<PaginatedModel<SubjectVM>>> GetAllSubjects(QueryModel queryModel);
        Task<ResultModel<SubjectVM>> AddSubject(SubjectInsertVM model);
        Task<ResultModel<List<SubjectVM>>> GetAllSubjects();
        Task<ResultModel<SubjectVM>> UpdateSubject(SubjectUpdateVM model);

        Task<ResultModel<string>> RemoveSubject(long SubjectId);
    }
}
