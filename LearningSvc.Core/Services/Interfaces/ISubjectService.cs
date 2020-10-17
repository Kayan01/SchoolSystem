using LearningSvc.Core.ViewModels.Subject;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningSvc.Core.Services.Interfaces
{
    public interface ISubjectService
    {
        Task<ResultModel<List<SubjectVM>>> GetAllSubjects();
        Task<ResultModel<SubjectVM>> AddSubject(SubjectVM model);
    }
}
