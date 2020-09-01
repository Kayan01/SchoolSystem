using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Auth.Core.ViewModels.SchoolClass;

namespace Auth.Core.Services.Interfaces
{
    public interface ISchoolClassService
    {
        /// <summary>
        /// Add class to school
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<ClassVM>> AddClass(ClassVM model);

        /// <summary>
        /// Add class group e.g jss1a b and c
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<ClassGroupVM>> AddClassGroup(ClassGroupVM model);
        /// <summary>
        /// Add sections e.g secondary, primary etc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<ClassSectionVM>> AddSection(ClassSectionVM model);

        /// <summary>
        /// adds student to class
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="studId"></param>
        /// <returns></returns>
        Task<ResultModel<string>> AddStudentToClass(ClassStudentVM vm);

        /// <summary>
        /// registers subject to class
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="subjId"></param>
        /// <returns></returns>
        Task<ResultModel<bool>> AssignSubjectToClass(ClassSubjectVM vm);

        /// <summary>
        /// registers teacher to class
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="staffId"></param>
        /// <returns></returns>
        Task<ResultModel<string>> AssignTeacherToClass(ClassTeacherVM vm);

        /// <summary>
        /// Remove class
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel<bool>> DeleteClass(long Id);

        /// <summary>
        /// Remove class group
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel<bool>> DeleteClassGroup(long Id);
        /// <summary>
        /// Remove section
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel<bool>> DeleteSection(long Id);


        /// <summary>
        /// Gets classes in school
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<List<ListClassVM>>> GetAllClasses();

        /// <summary>
        /// Gets all sections (e.g secondary, primary, nursery) in a school. 
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<List<ClassSectionVM>>> GetAllSections ();
        /// <summary>
        /// Gets class groups in school
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<List<ClassGroupVM>>> GetAllClassGroups();
        /// <summary>
        /// Get specific class in school
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel<ClassVM>> GetClassById(long Id);

        /// <summary>
        /// Get all students in a class
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel<ListClassVM>> GetClassByIdWithStudents(long Id);
        /// <summary>
        /// Update class details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<ClassUpdateVM>> UpdateClass(ClassUpdateVM model);
        /// <summary>
        /// Update section
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<ClassSectionUpdateVM>> UpdateSection(ClassSectionUpdateVM model);
        /// <summary>
        /// Update class group details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<ClassGroupVM>> UpdateClassGroup(ClassGroupVM model);
    }
}
