using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.ViewModels.SchoolClass;

namespace UserManagement.Core.Services.Interfaces
{
    public interface ISchoolClassService
    {
        /// <summary>
        /// Gets classes in school
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<object>> GetAllClasses(long Id);
        /// <summary>
        /// Get all students in a class
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel<object>> GetClassByIdWithStudents(long Id);
        /// <summary>
        /// Add class to school
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<ClassVM>> AddClass(ClassVM model);
        /// <summary>
        /// Get specific class in school
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel<ClassVM>> GetClassById(long Id);
        /// <summary>
        /// Update class details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<ClassUpdateVM>> UpdateSchool(ClassUpdateVM model);
        /// <summary>
        /// Remove class
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel<bool>> DeleteClass(long Id);

        /// <summary>
        /// adds student to class
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="studId"></param>
        /// <returns></returns>
        Task<ResultModel<object>> AddStudentToClass(long classId, long studId);
        /// <summary>
        /// registers subject to class
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="subjId"></param>
        /// <returns></returns>
        Task<ResultModel<bool>> AssignSubjectToClass(long classId, long subjId);
        /// <summary>
        /// registers teacher to class
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="staffId"></param>
        /// <returns></returns>
        Task<ResultModel<bool>> AssignTeacherToClass(long classId, long staffId);
    }
}
