using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth.Core.Models;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels.SchoolClass;
using Auth.Core.ViewModels;
using Auth.Core.Models.JoinTables;
using Auth.Core.Models.Users;

namespace Auth.Core.Services
{
    public class SchoolClassService : ISchoolClassService
    {
        private readonly IRepository<ClassGroup, long> _classGroupRepo;
        private readonly IRepository<SchoolClass, long> _classRepo;
        private readonly IRepository<SchoolSection, long> _sectionsRepo;
        private readonly IRepository<Staff, long> _staffRepo;
        private readonly IRepository<TeachingStaff, long> _teachingStaffRepo;
        private readonly IRepository<Student, long> _studentRepo;
        private readonly IUnitOfWork _unitOfWork;
        public SchoolClassService(IUnitOfWork unitOfWork, IRepository<SchoolClass, long> classRepo,
            IRepository<Student, long> studentRepo, IRepository<ClassGroup, long> classGroupRepo, 
            IRepository<SchoolSection, long> sectionsRepo, IRepository<Staff, long> staffRepo, IRepository<TeachingStaff, long> teachingStaffRepo)
        {
            _unitOfWork = unitOfWork;
            _classRepo = classRepo;
            _studentRepo = studentRepo;
            _classGroupRepo = classGroupRepo;
            _sectionsRepo = sectionsRepo;
            _staffRepo = staffRepo;
            _teachingStaffRepo = teachingStaffRepo;
        }
        public async Task<ResultModel<ClassVM>> AddClass(ClassVM model)
        {
            var result = new ResultModel<ClassVM>();


            //check if section exists
            var sc = await _sectionsRepo.FirstOrDefaultAsync(model.SectionId);
            if (sc == null)
            {
                result.AddError("School section does not exist.Please create school section");
                return result;
            }

            //check if school group exist
            var gp = await _classGroupRepo.FirstOrDefaultAsync(model.ClassGroupId);

            if (gp == null)
            {

                result.AddError("Class group does not exist. Please create the class group");
                return result;
            }



            //todo: add more props
            var cls = new SchoolClass { Name = model.Name, SchoolSectionId = model.SectionId };

            //link class to class group e.g link jss1 to A or B == jss1A or jss1B
            var clsgrp = new Class2Group { ClassGroupId = gp.Id, SchoolClass = cls };

            cls.ClassJoinGroup.Add(clsgrp);

            var id = _classRepo.InsertAndGetId(cls);
            await _unitOfWork.SaveChangesAsync();
            model.Id = id;
            result.Data = model;
            return result;
        }
        public async Task<ResultModel<ClassGroupVM>> AddClassGroup(ClassGroupVM model)
        {
            var result = new ResultModel<ClassGroupVM>();
            //todo: add more props
            var cg = _classGroupRepo.Insert(new ClassGroup { Name = model.Name });
            await _unitOfWork.SaveChangesAsync();
            model.Id = cg.Id;
            result.Data = model;
            return result;
        }
        public async Task<ResultModel<ClassSectionVM>> AddSection(ClassSectionVM model)
        {
            var result = new ResultModel<ClassSectionVM>();
            //todo: add more props
            var sc = _sectionsRepo.Insert(new SchoolSection { Name = model.Name });
            await _unitOfWork.SaveChangesAsync();
            model.Id = sc.Id;
            result.Data = model;
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
            await _classRepo.DeleteAsync(Id);
            await _unitOfWork.SaveChangesAsync();
            result.Data = true;

            return result;
        }
        public async Task<ResultModel<bool>> DeleteClassGroup(long Id)
        {
            var result = new ResultModel<bool> { Data = false };
            await _classGroupRepo.DeleteAsync(Id);
            await _unitOfWork.SaveChangesAsync();
            result.Data = true;

            return result;
        }

        public async Task<ResultModel<bool>> DeleteSection(long Id)
        {
            var result = new ResultModel<bool> { Data = false };
            await _sectionsRepo.DeleteAsync(Id);
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
        public async Task<ResultModel<List<ClassGroupVM>>> GetAllClassGroups()
        {
            var result = new ResultModel<List<ClassGroupVM>>
            {
                Data = await _classGroupRepo.GetAll().Select(x => (ClassGroupVM)x).ToListAsync()
            };
            return result;
        }
        public async Task<ResultModel<List<ClassSectionVM>>> GetAllSections()
        {
            var result = new ResultModel<List<ClassSectionVM>>
            {
                Data = await _sectionsRepo.GetAll().Select(x => (ClassSectionVM)x).ToListAsync()
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

            //gets class with students. Casting of students to studentsVM is done in the classVM
            var @class = await _classRepo.GetAll().Include(x => x.Students).Select(x => (ListClassVM)x).FirstOrDefaultAsync(x => x.Id == Id);

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
        public async Task<ResultModel<ClassGroupVM>> UpdateClassGroup(ClassGroupVM model)
        {
            var @class = await _classGroupRepo.FirstOrDefaultAsync(model.Id);
            var result = new ResultModel<ClassGroupVM>();

            if (@class == null)
            {
                result.AddError("Class could not be found");

                return result;
            }


            //TODO: add more props
            @class.Name = model.Name;



            await _classGroupRepo.UpdateAsync(@class);
            await _unitOfWork.SaveChangesAsync();
            result.Data = model;
            return result;

        }
        public async Task<ResultModel<ClassSectionUpdateVM>> UpdateSection(ClassSectionUpdateVM model)
        {
            var sec = await _sectionsRepo.FirstOrDefaultAsync(model.Id);
            var result = new ResultModel<ClassSectionUpdateVM>();

            if (sec == null)
            {
                result.AddError("Section could not be found");

                return result;
            }


            //TODO: add more props
            sec.Name = model.Name;



            await _sectionsRepo.UpdateAsync(sec);
            await _unitOfWork.SaveChangesAsync();
            result.Data = model;
            return result;
        }
    }
}
