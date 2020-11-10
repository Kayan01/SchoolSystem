using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Core.Models;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels.SchoolClass;
using Auth.Core.Models.Users;
using Shared.PubSub;

namespace Auth.Core.Services
{
    public class SchoolClassService : ISchoolClassService
    {
        private readonly IRepository<ClassArm, long> _classArmRepo;
        private readonly IRepository<SchoolClass, long> _classRepo;
        private readonly IRepository<SchoolSection, long> _schoolSectionsRepo;
        private readonly IRepository<TeachingStaff, long> _teachingStaffRepo;
        private readonly IRepository<Student, long> _studentRepo;
        private readonly IPublishService _publishService;
        private readonly IUnitOfWork _unitOfWork;

        public SchoolClassService(IUnitOfWork unitOfWork, IRepository<SchoolClass, long> classRepo,
            IRepository<Student, long> studentRepo, IRepository<ClassArm, long> classGroupRepo,
            IRepository<SchoolSection, long> sectionsRepo,
            IPublishService publishService,
            IRepository<TeachingStaff, long> teachingStaffRepo)
        {
            _unitOfWork = unitOfWork;
            _classRepo = classRepo;
            _studentRepo = studentRepo;
            _classArmRepo = classGroupRepo;
            _schoolSectionsRepo = sectionsRepo;
            _teachingStaffRepo = teachingStaffRepo;
            _publishService = publishService;
        }

        public async Task<ResultModel<ClassVM>> AddClass(ClassVM model)
        {
            var result = new ResultModel<ClassVM>();

            //check if section exists
            var schoolSection = await _schoolSectionsRepo.FirstOrDefaultAsync(model.SectionId);
            if (schoolSection == null)
            {
                result.AddError("School section does not exist.Please create school section");
                return result;
            }

            //check if class arm exist
            var classArm = await _classArmRepo.FirstOrDefaultAsync(model.ClassGroupId);

            if (classArm == null)
            {
                result.AddError("Class arm does not exist. Please create the class arm");
                return result;
            }

            //todo: add more props
            var cls = new SchoolClass
            {
                Name = model.Name,
                ClassArm = classArm.Name,
                SchoolSectionId = model.SectionId
            };

            var id = _classRepo.InsertAndGetId(cls);
            await _unitOfWork.SaveChangesAsync();
            model.Id = id;
            result.Data = model;

            //PublishMessage
            await _publishService.PublishMessage(Topics.Class, BusMessageTypes.CLASS, new ClassSharedModel
            {
                Id = cls.Id,
                TenantId = cls.TenantId,
                Name = cls.Name,
                ClassArm = cls.ClassArm
            });

            return result;
        }

        public async Task<ResultModel<string>> AddStudentToClass(ClassStudentVM vm)
        {
            var result = new ResultModel<string>();

            //get student
            var stud = await _studentRepo.FirstOrDefaultAsync(vm.StudId);

            if (stud == null)
            {
                result.AddError("Student not found");
                return result;
            }

            //get class
            var @class = await _classRepo.FirstOrDefaultAsync(vm.ClassId);

            if (@class == null)
            {
                result.AddError("Class not found");
                return result;
            }

            //assign student to class
            stud.ClassId = vm.ClassId;

            await _unitOfWork.SaveChangesAsync();

            result.Message = "Student added to class successfully";
            return result;
        }

        public Task<ResultModel<bool>> AssignSubjectToClass(ClassSubjectVM vm)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultModel<string>> AssignTeacherToClass(ClassTeacherVM vm)
        {
            var result = new ResultModel<string>();

            //get staff
            var staff = await _teachingStaffRepo.FirstOrDefaultAsync(vm.TeacherId);

            if (staff == null)
            {
                result.AddError("Staff not found");
                return result;
            }

            //get class
            var @class = await _classRepo.FirstOrDefaultAsync(vm.ClassId);

            if (@class == null)
            {
                result.AddError("Class not found");
                return result;
            }

            //assign student to class
            staff.ClassId = vm.ClassId;

            await _unitOfWork.SaveChangesAsync();

            result.Message = "Staff assigned to class successfully";
            return result;
        }

        public async Task<ResultModel<bool>> DeleteClass(long Id)
        {
            var result = new ResultModel<bool> { Data = false };

            //check if class exists
            var class_ = await _classRepo
                .GetAllIncluding(x => x.Students)
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();

            if (class_ == null)
            {
                result.AddError($"Class does not exist");

                return result;
            }

            //check if student are in class
            if (class_.Students.Count > 0)
            {
                result.AddError($"Class cannot be deleted. Students exist in class");

                return result;
            }

            await _classRepo.DeleteAsync(Id);
            await _unitOfWork.SaveChangesAsync();
            result.Data = true;

            return result;
        }

        public async Task<ResultModel<List<ListClassVM>>> GetAllClasses()
        {
            var result = new ResultModel<List<ListClassVM>>
            {
                Data = await _classRepo.GetAll().Select(x => (ListClassVM)x).ToListAsync()
            };
            return result;
        }

        public async Task<ResultModel<ClassVM>> GetClassById(long Id)
        {
            var result = new ResultModel<ClassVM>();
            var @class = (await _classRepo.FirstOrDefaultAsync(Id));

            if (@class == null)
            {
                result.AddError("Class does not exist");
                return result;
            }

            result.Data = @class;
            return result;
        }

        public async Task<ResultModel<ListClassVM>> GetClassByIdWithStudents(long Id)
        {
            var result = new ResultModel<ListClassVM>();

            //gets class with students.
            var @class = await _classRepo.GetAll()
                .Include(x => x.Students)
                .Where(x => x.Id == Id)
                .Select(x => (ListClassVM)x)
                .FirstOrDefaultAsync();

            if (@class == null)
            {
                result.AddError("Class does not exist");
                return result;
            }

            result.Data = @class;
            return result;
        }

        public async Task<ResultModel<ClassUpdateVM>> UpdateClass(ClassUpdateVM model)
        {
            var @class = await _classRepo.FirstOrDefaultAsync(model.Id);
            var result = new ResultModel<ClassUpdateVM>();

            if (@class == null)
            {
                result.AddError("Class could not be found");

                return result;
            }

            //TODO: add more props
            @class.Name = model.Name;

            await _classRepo.UpdateAsync(@class);
            await _unitOfWork.SaveChangesAsync();
            result.Data = model;
            return result;
        }
    }
}