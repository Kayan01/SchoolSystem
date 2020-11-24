using Auth.Core.ViewModels.SchoolClass;
using Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auth.Core.Services.Interfaces.Class
{
    public interface IClassArmService
    {
        Task<ResultModel<ClassArmVM>> AddClassArm(AddClassArm model);

        Task<ResultModel<bool>> DeleteClassArm(long Id);

        Task<ResultModel<List<ClassArmVM>>> GetAllClassArms();
        Task<ResultModel<ClassArmVM>> GetAllClassArmById(long Id);

        Task<ResultModel<ClassArmVM>> UpdateClassArm(UpdateClassArmVM model, long id);
    }
}