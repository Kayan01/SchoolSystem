using Auth.Core.ViewModels.SchoolClass;
using Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auth.Core.Services.Interfaces
{
    public interface ISchoolClassService
    {
        Task<ResultModel<ClassVM>> AddClass(ClassVM model);

        Task<ResultModel<string>> AddStudentToClass(ClassStudentVM vm);

        Task<ResultModel<bool>> AssignSubjectToClass(ClassSubjectVM vm);

        Task<ResultModel<string>> AssignTeacherToClass(ClassTeacherVM vm);

        Task<ResultModel<bool>> DeleteClass(long Id);

        Task<ResultModel<List<ListClassVM>>> GetAllClasses();

        Task<ResultModel<ClassVM>> GetClassById(long Id);

        Task<ResultModel<ListClassVM>> GetClassByIdWithStudents(long Id);

        Task<ResultModel<ClassUpdateVM>> UpdateClass(ClassUpdateVM model);
    }
}