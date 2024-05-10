using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Models;
using AssessmentSvc.Core.ViewModels.Student;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Services
{
    public class StudentService : IStudentService
    {
        private readonly IRepository<Student, long> _studentRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StudentService> _logger;

        public StudentService(IUnitOfWork unitOfWork, IRepository<Student, long> studentRepo, ILogger<StudentService> logger)
        {
            _unitOfWork = unitOfWork;
            _studentRepo = studentRepo;
            _logger = logger;
        }


        public async void AddOrUpdateStudentFromBroadcast(List<StudentSharedModel> models)
        {
            _logger.LogInformation("Start insert or update of student records");
            var studentUpdateList = new List<Student>();


            var idList = models.Select(m => m.Id).Distinct();
            var studentList = _studentRepo.GetAll().Where(m => idList.Contains(m.Id) && m.IsDeleted == false).ToList();

            foreach (var model in models)
            {
                var student = studentList.FirstOrDefault(x => x.Id == model.Id && x.TenantId == model.TenantId);
                if (student == null)
                {
                    student = _studentRepo.Insert(new Student
                    {
                        Id = model.Id
                    });
                }
                else
                {
                    await _studentRepo.UpdateAsync(student);
                }

                student.TenantId = model.TenantId;
                student.ClassId = model.ClassId;
                student.FirstName = string.IsNullOrEmpty(model.FirstName) ? student.FirstName : model.FirstName;
                student.LastName = string.IsNullOrEmpty(model.LastName) ? student.LastName : model.LastName;
                student.Email = string.IsNullOrEmpty(model.Email) ? student.Email : model.Email;
                student.Phone = string.IsNullOrEmpty(model.Phone) ? student.Phone : model.Phone;
                student.ParentEmail = string.IsNullOrEmpty(model.ParentEmail) ? student.ParentEmail : model.ParentEmail;
                student.ParentName = string.IsNullOrEmpty(model.ParentName) ? student.ParentName : model.ParentName;
                student.UserId = model.UserId;
                student.RegNumber = model.RegNumber;
                student.IsActive = model.IsActive;
                student.IsDeleted = model.IsDeleted;
                student.Sex = model.Sex;
                student.DateOfBirth = model.DoB;
                student.StudentStatusInSchool = model.StudentStatusInSchool;
            }

            _unitOfWork.SaveChanges();

            _logger.LogInformation("Student records successfuly added to Assessment table");
            
        }

        public async Task<long> GetStudentClassIdByUserId(long userId)
        {
            var student = await _studentRepo.GetAll().Where(m => m.UserId == userId && m.IsDeleted == false).FirstOrDefaultAsync();
            if (student == null)
            {
                return 0;
            }
            return student.ClassId.Value;
        }

        public async Task<ResultModel<List<StudentVM>>> GetStudentsByClass(long classId)
        {
            var students = await _studentRepo.GetAll()
                .Where(m => m.ClassId == classId && m.IsDeleted == false)
                .Select(m=>(StudentVM)m).ToListAsync();

            return new ResultModel<List<StudentVM>>()
            {
                Data = students
            };
        }

        public async Task<ResultModel<List<StudentParentMailingInfo>>> GetParentsMailInfo(long[] ids)
        {
            var info = await _studentRepo.GetAll()
                .Where(m => ids.Any(n=>n == m.Id) && m.IsDeleted == false)
                .Select(m => new StudentParentMailingInfo()
                {
                    Email= m.ParentEmail,
                    ParentName = m.ParentName,
                    StudentId = m.Id,
                    StudentName = $"{m.FirstName} {m.LastName}"
                }).ToListAsync();

            return new ResultModel<List<StudentParentMailingInfo>>()
            {
                Data = info
            };
        }

    }
}
