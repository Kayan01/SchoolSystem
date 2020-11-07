using LearningSvc.Core.ViewModels.ClassSubject;
using LearningSvc.Core.ViewModels.SchoolClass;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningSvc.Core.Interfaces
{
    public interface IClassSubjectService
    {
        Task<ResultModel<string>> AddSubjectsForClass(ClassSubjectsInsertVM model);
        Task<ResultModel<List<ClassSubjectListVM>>> GetAllClassSubjects();
    }
}
