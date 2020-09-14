using Auth.Core.ViewModels.SchoolClass;
using Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auth.Core.Services.Interfaces.Class
{
    public interface ISectionService
    {
        Task<ResultModel<ClassSectionVM>> AddSection(ClassSectionVM model);

        Task<ResultModel<bool>> DeleteSection(long Id);

        Task<ResultModel<List<ClassSectionVM>>> GetAllSections();

        Task<ResultModel<ClassSectionUpdateVM>> UpdateSection(ClassSectionUpdateVM model);
    }
}