using Auth.Core.ViewModels.SchoolClass;
using Shared.Pagination;
using Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auth.Core.Services.Interfaces.Class
{
    public interface ISectionService
    {
        Task<ResultModel<ClassSectionVM>> AddSection(ClassSectionVM model);
        Task<ResultModel<ClassSectionVM>> GetSectionById(long Id);

        Task<ResultModel<bool>> DeleteSection(long Id);

        Task<ResultModel<PaginatedModel<ClassSectionVM>>> GetAllSections(QueryModel vm);

        Task<ResultModel<ClassSectionUpdateVM>> UpdateSection(long id, ClassSectionUpdateVM model);
    }
}