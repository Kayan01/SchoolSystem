using LearningSvc.Core.Interfaces;
using LearningSvc.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningSvc.Core.Services
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


        public void AddOrUpdateStudentFromBroadcast(List<StudentSharedModel> models)
        {

            _logger.LogInformation("Start insert or update of student records");

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

                student.TenantId = model.TenantId;
                student.ClassId = model.ClassId;
                student.FirstName = string.IsNullOrEmpty(model.FirstName) ? student.FirstName : model.FirstName;
                student.LastName = string.IsNullOrEmpty(model.LastName) ? student.LastName : model.LastName;
                student.Email = string.IsNullOrEmpty(model.Email) ? student.Email : model.Email;
                student.Phone = string.IsNullOrEmpty(model.Phone) ? student.Phone : model.Phone;
                student.ParentEmail = string.IsNullOrEmpty(model.ParentEmail) ? student.ParentEmail : model.ParentEmail;
                student.ParentName = string.IsNullOrEmpty(model.ParentName) ? student.ParentName : model.ParentName;
                student.ParentId = model.ParentId == 0 ? student.ParentId : model.ParentId;
                student.UserId = model.UserId;
                student.IsActive = model.IsActive;
                student.IsDeleted = model.IsDeleted;
                student.RegNumber = model.RegNumber;
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
        public async Task<long> GetStudentIdByUserId(long userId)
        {
            var student = await _studentRepo.GetAll().Where(m => m.UserId == userId && m.IsDeleted == false).FirstOrDefaultAsync();
            if (student == null)
            {
                return 0;
            }
            return student.Id;
        }
    }
}
