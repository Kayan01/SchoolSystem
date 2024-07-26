using FinanceSvc.Core.Context;
using FinanceSvc.Core.Models;
using FinanceSvc.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FinanceSvc.Core.Services
{
    public class BroadCastDataService : IBroadcastDateService
    {
        private readonly IRepository<Models.School, long> _schoolRepo;
        private readonly IRepository<Models.Student, long> _studentRepo;
        private readonly IRepository<Models.SchoolClass, long> _schoolClassRepo;
        private readonly IRepository<Models.Parent, long> _parentRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        // private readonly BaseUrl _baseUrl;

        public BroadCastDataService(IRepository<Models.School, long> schoolRepo, 
        IRepository<Models.Student, long> studentRepo, 
        IRepository<Models.SchoolClass, long> schoolClassRepo,
        IRepository<Models.Parent, long> parentRepo,
        IUnitOfWork unitOfWork,
        AppDbContext context
        //BaseUrl baseUrl
        )
        {
            _schoolRepo = schoolRepo;
            _studentRepo = studentRepo;
            _parentRepo = parentRepo;
            _schoolClassRepo = schoolClassRepo;
            _unitOfWork = unitOfWork;
            _context = context;
            //_baseUrl = baseUrl;
        }

        public async Task<ResultModel<string>> GetDataFromAuth(DateTime startDate,  DateTime endDate)
        {
            var resultModel = new ResultModel<string>();
            var studentList = new List<Student>();  
            var parentList = new List<Parent>();  
            var schoolList = new List<School>();  
            var classList = new List<SchoolClass>();  

            HttpClient httpClient = new HttpClient();

            var response = await httpClient.GetAsync($"http://108.181.198.66:58100/schtrack-auth/api/v1/Student/GetDataFromAuthToFInance?startDate={startDate}&endDate={endDate}");

            var responseFromServer = await response.Content.ReadAsStringAsync();
            if ((int)response.StatusCode == 200)
            {
                try
                {
                    
                    var Data = JsonConvert.DeserializeObject<ApiResponse<FinanceObJ>>(responseFromServer);
                    var SerializeData = Data.Payload;

                    foreach (var school in SerializeData.Schools)
                    {
                        var schoolCheck = await _schoolRepo.FirstOrDefaultAsync(x => x.Id == school.Id);
                        if (schoolCheck == null)
                        {
                            var schoolData = new School
                            {
                                Id = school.Id,
                                CreationTime = DateTime.Now,
                                Email = school.Email,
                                IsActive = school.IsActive,
                                EmailPassword = school.EmailPassword,
                                Address = school.Address,
                                City = school.City,
                                DomainName = school.DomainName,
                                Name = school.Name,
                                PhoneNumber = school.PhoneNumber,
                                State = school.State,
                                WebsiteAddress = school.WebsiteAddress
                            };
                            schoolList.Add(schoolData);

                            _context.Schools.Add(schoolData);
                        }
                    }
                    _context.SaveChanges();

                    foreach (var parent in SerializeData.Parents)
                    {
                        var parentCheck = await _parentRepo.FirstOrDefaultAsync(x => x.Id == parent.Id);
                        if (parentCheck == null)
                        {
                            var parentData = new Parent
                            {
                                Id = parent.Id,
                                FirstName = parent.FirstName,
                                LastName = parent.LastName,
                                CreationTime = DateTime.Now,
                                Email = parent.Email,
                                IsActive = parent.IsActive,
                                IsDeleted = parent.IsDeleted,
                                RegNumber = parent.RegNumber,
                                Phone = parent.Phone,
                                UserId = parent.UserId,
                                SecondaryEmail = parent.SecondaryEmail,
                                HomeAddress = parent.HomeAddress,
                                OfficeAddress = parent.OfficeAddress,
                                SecondaryPhoneNumber = parent.SecondaryPhoneNumber,

                            };
                            parentList.Add(parentData);
                        }
                    }

                    foreach (var singleClass in SerializeData.CLasses)
                    {
                        var classCheck = await _context.Classes.FirstOrDefaultAsync(x => x.Id == singleClass.Id);
                        if (classCheck == null)
                        {
                            var schoolClassData = new SchoolClass
                            {
                                Id = singleClass.Id,
                                CreationTime = DateTime.Now,
                                Name = singleClass.Name,
                                ClassArm = singleClass.ClassArm,
                                TenantId = singleClass.TenantId,
                            };

                            classList.Add(schoolClassData);        
                        }
                    }
                    


                    foreach (var student in SerializeData.Students)
                    {
                        var studentCheck = await _context.Students.FirstOrDefaultAsync(x => x.Id == student.Id);
                        if (studentCheck == null)
                        {
                            var studentData = new Student
                            {
                                Id = student.Id,
                                FirstName = student.FirstName,
                                LastName = student.LastName,
                                ClassId = student.ClassId,
                                CreationTime = DateTime.Now,
                                Email = student.Email,
                                IsActive = student.IsActive,
                                IsDeleted = student.IsDeleted,
                                ParentId = student.ParentId,
                                RegNumber = student.RegNumber,
                                Phone = student.Phone,
                                TenantId = student.TenantId,
                                UserId = student.UserId,
                                StudentStatusInSchool = student.StudentStatusInSchool
                            };
                            studentList.Add(studentData);
                           
                        }
                    }
                    _context.Schools.AddRange(schoolList);
                    _context.Parents.AddRange(parentList);
                    _context.Classes.AddRange(classList);
                    _context.Students.AddRange(studentList);

                    _context.SaveChanges();

             
                }
                catch (Exception ex)
                {
                    resultModel.AddError($"Error Occured During Data Insertion {ex.Message}");
                    return resultModel;
                }
               // _context.SaveChanges();
                

                resultModel.Data = "Successful";
                resultModel.Message = "Data Populated Successfully";

            }
            else
            {
                resultModel.AddError("Api Call Unsuccessful");
            }
            
            return resultModel;
        }
    }
}
