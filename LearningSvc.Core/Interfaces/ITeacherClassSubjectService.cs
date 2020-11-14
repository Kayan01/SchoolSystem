using LearningSvc.Core.ViewModels.SchoolClass;
using LearningSvc.Core.ViewModels.TeacherClassSubject;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningSvc.Core.Interfaces
{
    public interface ITeacherClassSubjectService
    {
        Task<ResultModel<string>> AddTeacherToClassSubjects(TeacherClassSubjectInsertVM model);
        Task<ResultModel<List<TeacherClassSubjectListVM>>> GetAllTeacherClassSubjects(long teacherId);
        Task<ResultModel<List<TeacherClassSubjectListVM>>> GetTeachersForClassSubject(long classSubjectId);
    }
}
