using Auth.Core.ViewModels.SchoolClass;
using Shared.Pagination;
using Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auth.Core.Services.Interfaces
{
    public interface ISchoolClassService
    {
        Task<ResultModel<string>> AddClass(AddClassVM model);

        Task<ResultModel<string>> AddStudentToClass(ClassStudentVM vm);

        Task<ResultModel<bool>> AssignSubjectToClass(ClassSubjectVM vm);

        Task<ResultModel<bool>> DeleteClass(long Id);

        Task<ResultModel<PaginatedModel<ListClassVM>>> GetAllClasses(QueryModel vm);

        Task<ResultModel<ClassVM>> GetClassById(long Id);
        Task<ResultModel<List<ListClassVM>>> GetClassBySection(long levelId);

        Task<ResultModel<ListClassVM>> GetClassByIdWithStudents(long Id);
        Task<ResultModel<List<ClassWithoutArmVM>>> GetClassesWithoutArm();
        Task<ResultModel<string>> UpdateClassSequenceAndTerminal(List<ClassWithoutArmVM> model);

        Task<ResultModel<ClassUpdateVM>> UpdateClass(ClassUpdateVM model);
    }
}