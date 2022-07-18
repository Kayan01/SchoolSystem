using Auth.Core.Test.Services.Setup;
using Auth.Core.ViewModels.Student;
using NUnit.Framework;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels.Parent;
using Auth.Core.ViewModels.Setup;
using Auth.Core.Interfaces.Users;
using Auth.Core.Interfaces.Setup;
using Shared.ViewModels;
using Auth.Core.Services.Interfaces.Class;
using Auth.Core.Context;
using Auth.Core.ViewModels.SchoolClass;
using Auth.Core.Models;
using Auth.Core.ViewModels;
using System.Linq;
using Auth.Core.Models.Contact;

namespace Auth.Core.Test.Services.Users
{
    [TestFixture]
    class StudentServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }
         
        [Test]
        public async Task AddStudent_Test()
        {
            using ServicesDISetup _setup = new ServicesDISetup();
            var studentService = _setup.ServiceProvider.GetService<IStudentService>();
            var ParentService = _setup.ServiceProvider.GetService<IParentService>();
            var SchoolPropertyService = _setup.ServiceProvider.GetService<ISchoolPropertyService>();
            var ClassService = _setup.ServiceProvider.GetService<ISchoolClassService>();
            var _sectionService = _setup.ServiceProvider.GetService<ISectionService>();
            var _classArmService = _setup.ServiceProvider.GetService<IClassArmService>();
            var context = _setup.ServiceProvider.GetService<AppDbContext>();


            var GetAllParent = GetAllParentData();

            var SectionModel = AddClassSectionData();
            var ClassArmModel = AddClassArmData();
            var ClassModel = AddClassData();

            var school = AddSchoolData();
            context.Schools.Add(school);
            await context.SaveChangesAsync();

            var AddSection = await _sectionService.AddSection(SectionModel);
            var AddClassArm = await _classArmService.AddClassArm(ClassArmModel);

            ClassModel.SectionId = AddSection.Data.Id;

            List<long> ListOfClassArms = new List<long>()
            {
                AddClassArm.Data.Id
            };
            ClassModel.ClassArmIds = ListOfClassArms;

            var AddClass = await ClassService.AddClass(ClassModel);

            var schoolPropertyData = SchoolPropertyVMData();

            var AddSchoolProperty = await SchoolPropertyService.SetSchoolProperty(schoolPropertyData);
            Assert.AreEqual(AddSchoolProperty.Data.EnrollmentAmount, schoolPropertyData.EnrollmentAmount);

            var data = AddParentVMData();

            var AddParent = ParentService.AddNewParent(data);
            var studentData = AddStudentData();
            studentData.ParentId = 1;
            studentData.SectionId = AddSection.Data.Id;
            studentData.ClassId = 3;

            var result = await studentService.AddStudentToSchool(studentData);
            var query = QueryModelData();
            var getALlStudent = await studentService.GetStudentById(result.Data.Id);

            Assert.That(getALlStudent.Data.RegNumber == result.Data.StudentNumber);
            Assert.AreEqual(result.Data.UserId, getALlStudent.Data.UserId);
            Assert.AreEqual(result.Data.Id, getALlStudent.Data.Id);
            Assert.AreEqual(result.Data.StudentNumber, getALlStudent.Data.RegNumber);
            Assert.AreEqual(result.Data.Email,studentData.ContactEmail);
            
        }

        [Test]
        public async Task AddStudent_Class_Does_Not_Exist_Test()
        {
            using ServicesDISetup _setup = new ServicesDISetup();
            var studentService = _setup.ServiceProvider.GetService<IStudentService>();
            var ParentService = _setup.ServiceProvider.GetService<IParentService>();
            var SchoolPropertyService = _setup.ServiceProvider.GetService<ISchoolPropertyService>();
            
            var context = _setup.ServiceProvider.GetService<AppDbContext>();


            var GetAllParent = GetAllParentData();

            var SectionModel = AddClassSectionData();
            var ClassArmModel = AddClassArmData();
            var ClassModel = AddClassData();

            var school = AddSchoolData();
            context.Schools.Add(school);
            await context.SaveChangesAsync();

            var schoolPropertyData = SchoolPropertyVMData();

            var AddSchoolProperty = await SchoolPropertyService.SetSchoolProperty(schoolPropertyData);
            Assert.AreEqual(AddSchoolProperty.Data.EnrollmentAmount, schoolPropertyData.EnrollmentAmount);

            var data = AddParentVMData();

            var AddParent = ParentService.AddNewParent(data);
            var studentData = AddStudentData();
            studentData.ParentId = 1;
            studentData.ClassId = 3;

            var result = await studentService.AddStudentToSchool(studentData);

            Assert.That(result.ErrorMessages.Contains("class does not exists"));
        }

        [Test]
        public async Task AddStudent_Parent_Do_Not_Exist_Test()
        {
            using ServicesDISetup _setup = new ServicesDISetup();
            var studentService = _setup.ServiceProvider.GetService<IStudentService>();
            var SchoolPropertyService = _setup.ServiceProvider.GetService<ISchoolPropertyService>();
            var ClassService = _setup.ServiceProvider.GetService<ISchoolClassService>();
            var _sectionService = _setup.ServiceProvider.GetService<ISectionService>();
            var _classArmService = _setup.ServiceProvider.GetService<IClassArmService>();
            var context = _setup.ServiceProvider.GetService<AppDbContext>();


            var SectionModel = AddClassSectionData();
            var ClassArmModel = AddClassArmData();
            var ClassModel = AddClassData();

            var school = AddSchoolData();
            context.Schools.Add(school);
            await context.SaveChangesAsync();

            var AddSection = await _sectionService.AddSection(SectionModel);
            var AddClassArm = await _classArmService.AddClassArm(ClassArmModel);

            ClassModel.SectionId = AddSection.Data.Id;

            List<long> ListOfClassArms = new List<long>()
            {
                AddClassArm.Data.Id
            };
            ClassModel.ClassArmIds = ListOfClassArms;

            var AddClass = await ClassService.AddClass(ClassModel);

            var schoolPropertyData = SchoolPropertyVMData();

            var AddSchoolProperty = await SchoolPropertyService.SetSchoolProperty(schoolPropertyData);
            Assert.AreEqual(AddSchoolProperty.Data.EnrollmentAmount, schoolPropertyData.EnrollmentAmount);

            var studentData = AddStudentData();
            studentData.ParentId = 1;
            studentData.SectionId = AddSection.Data.Id;
            studentData.ClassId = 3;

            var result = await studentService.AddStudentToSchool(studentData);

            Assert.That(result.ErrorMessages.Contains("No parent exists"));
        }

        [Test]
        public async Task DeleteStudent_Test()
        {
            using ServicesDISetup _Setup = new ServicesDISetup();
            var studentService = _Setup.ServiceProvider.GetService<IStudentService>();
            var context = _Setup.ServiceProvider.GetService<AppDbContext>();
            var ParentService = _Setup.ServiceProvider.GetService<IParentService>();
            var SchoolPropertyService = _Setup.ServiceProvider.GetService<ISchoolPropertyService>();
            var ClassService = _Setup.ServiceProvider.GetService<ISchoolClassService>();
            var _sectionService = _Setup.ServiceProvider.GetService<ISectionService>();
            var _classArmService = _Setup.ServiceProvider.GetService<IClassArmService>();

            var GetAllParent = GetAllParentData();

            var SectionModel = AddClassSectionData();
            var ClassArmModel = AddClassArmData();
            var ClassModel = AddClassData();

            var school = AddSchoolData();
            context.Schools.Add(school);
            await context.SaveChangesAsync();

            var AddSection = await _sectionService.AddSection(SectionModel);
            var AddClassArm = await _classArmService.AddClassArm(ClassArmModel);

            ClassModel.SectionId = AddSection.Data.Id;

            List<long> ListOfClassArms = new List<long>()
            {
                AddClassArm.Data.Id
            };
            ClassModel.ClassArmIds = ListOfClassArms;

            var AddClass = await ClassService.AddClass(ClassModel);

            var schoolPropertyData = SchoolPropertyVMData();

            var AddSchoolProperty = await SchoolPropertyService.SetSchoolProperty(schoolPropertyData);
            Assert.AreEqual(AddSchoolProperty.Data.EnrollmentAmount, schoolPropertyData.EnrollmentAmount);

            var data = AddParentVMData();

            var AddParent = ParentService.AddNewParent(data);
            var studentData = AddStudentData();
            studentData.ParentId = 1;
            studentData.SectionId = AddSection.Data.Id;
            studentData.ClassId = 3;

            var AddStudent = await studentService.AddStudentToSchool(studentData);

            var newAlumni = new DeactivateStudent
            {
                SessionName = "session",
                DeactivationReason = "Promoted"
            };

            var DeleteStudent = await studentService.DeleteStudent(AddStudent.Data.Id, newAlumni);

            Assert.That(!(DeleteStudent.ErrorMessages.Contains("Student not found")));
            Assert.That(DeleteStudent.Data == true);
        }
        [Test]
        public async Task DeleteStudent_Student_Not_Found_Test()
        {
            using ServicesDISetup _Setup = new ServicesDISetup();
            var studentService = _Setup.ServiceProvider.GetService<IStudentService>();
            var context = _Setup.ServiceProvider.GetService<AppDbContext>();
            var ParentService = _Setup.ServiceProvider.GetService<IParentService>();
            var SchoolPropertyService = _Setup.ServiceProvider.GetService<ISchoolPropertyService>();
            var ClassService = _Setup.ServiceProvider.GetService<ISchoolClassService>();
            var _sectionService = _Setup.ServiceProvider.GetService<ISectionService>();
            var _classArmService = _Setup.ServiceProvider.GetService<IClassArmService>();

            var GetAllParent = GetAllParentData();

            var SectionModel = AddClassSectionData();
            var ClassArmModel = AddClassArmData();
            var ClassModel = AddClassData();

            var school = AddSchoolData();
            context.Schools.Add(school);
            await context.SaveChangesAsync();

            var AddSection = await _sectionService.AddSection(SectionModel);
            var AddClassArm = await _classArmService.AddClassArm(ClassArmModel);

            ClassModel.SectionId = AddSection.Data.Id;

            List<long> ListOfClassArms = new List<long>()
            {
                AddClassArm.Data.Id
            };
            ClassModel.ClassArmIds = ListOfClassArms;

            var AddClass = await ClassService.AddClass(ClassModel);

            var schoolPropertyData = SchoolPropertyVMData();

            var AddSchoolProperty = await SchoolPropertyService.SetSchoolProperty(schoolPropertyData);
            Assert.AreEqual(AddSchoolProperty.Data.EnrollmentAmount, schoolPropertyData.EnrollmentAmount);

            var data = AddParentVMData();

            var AddParent = ParentService.AddNewParent(data);
            var studentData = AddStudentData();
            studentData.ParentId = 1;
            studentData.SectionId = AddSection.Data.Id;
            studentData.ClassId = 3;

            var AddStudent = await studentService.AddStudentToSchool(studentData);

            var newAlumni = new DeactivateStudent
            {
                SessionName = "session",
                DeactivationReason = "Promoted"
            };


            var Removestudent = await studentService.DeleteStudent(AddStudent.Data.Id, newAlumni);

            var DeleteStudent = await studentService.DeleteStudent(AddStudent.Data.Id, newAlumni);

            Assert.That(DeleteStudent.ErrorMessages.Contains("Student not found"));
            Assert.That(DeleteStudent.Data == false);
        }

        [Test]
        public async Task GetAllStudentsInSchool_Test()
        {

            using ServicesDISetup _Setup = new ServicesDISetup();
            var studentService = _Setup.ServiceProvider.GetService<IStudentService>();
            var context = _Setup.ServiceProvider.GetService<AppDbContext>();
            var ParentService = _Setup.ServiceProvider.GetService<IParentService>();
            var SchoolPropertyService = _Setup.ServiceProvider.GetService<ISchoolPropertyService>();
            var ClassService = _Setup.ServiceProvider.GetService<ISchoolClassService>();
            var _sectionService = _Setup.ServiceProvider.GetService<ISectionService>();
            var _classArmService = _Setup.ServiceProvider.GetService<IClassArmService>();

            var GetAllParent = GetAllParentData();

            var SectionModel = AddClassSectionData();
            var ClassArmModel = AddClassArmData();
            var ClassModel = AddClassData();
            var queryModel = QueryModelData();

            var school = AddSchoolData();
            context.Schools.Add(school);
            await context.SaveChangesAsync();

            var AddSection = await _sectionService.AddSection(SectionModel);
            var AddClassArm = await _classArmService.AddClassArm(ClassArmModel);

            ClassModel.SectionId = AddSection.Data.Id;

            List<long> ListOfClassArms = new List<long>()
            {
                AddClassArm.Data.Id
            };
            ClassModel.ClassArmIds = ListOfClassArms;

            var AddClass = await ClassService.AddClass(ClassModel);

            var schoolPropertyData = SchoolPropertyVMData();

            var AddSchoolProperty = await SchoolPropertyService.SetSchoolProperty(schoolPropertyData);
            Assert.AreEqual(AddSchoolProperty.Data.EnrollmentAmount, schoolPropertyData.EnrollmentAmount);

            var data = AddParentVMData();

            var AddParent = ParentService.AddNewParent(data);
            var studentData = AddStudentData();
            studentData.ParentId = 1;
            studentData.SectionId = AddSection.Data.Id;
            studentData.ClassId = 3;

            var AddStudent = await studentService.AddStudentToSchool(studentData);
            var GetAllStudent = await studentService.GetAllStudentsInSchool(queryModel);


            Assert.That(GetAllStudent.Data.PageSize > 0);
        }

        [Test]
        public async Task GetStudentById_Test()
        {
            using ServicesDISetup _Setup = new ServicesDISetup();
            var studentService = _Setup.ServiceProvider.GetService<IStudentService>();
            var context = _Setup.ServiceProvider.GetService<AppDbContext>();
            var ParentService = _Setup.ServiceProvider.GetService<IParentService>();
            var SchoolPropertyService = _Setup.ServiceProvider.GetService<ISchoolPropertyService>();
            var ClassService = _Setup.ServiceProvider.GetService<ISchoolClassService>();
            var _sectionService = _Setup.ServiceProvider.GetService<ISectionService>();
            var _classArmService = _Setup.ServiceProvider.GetService<IClassArmService>();

            var GetAllParent = GetAllParentData();

            var SectionModel = AddClassSectionData();
            var ClassArmModel = AddClassArmData();
            var ClassModel = AddClassData();

            var school = AddSchoolData();
            context.Schools.Add(school);
            await context.SaveChangesAsync();

            var AddSection = await _sectionService.AddSection(SectionModel);
            var AddClassArm = await _classArmService.AddClassArm(ClassArmModel);

            ClassModel.SectionId = AddSection.Data.Id;

            List<long> ListOfClassArms = new List<long>()
            {
                AddClassArm.Data.Id
            };
            ClassModel.ClassArmIds = ListOfClassArms;

            var AddClass = await ClassService.AddClass(ClassModel);

            var schoolPropertyData = SchoolPropertyVMData();

            var AddSchoolProperty = await SchoolPropertyService.SetSchoolProperty(schoolPropertyData);
            Assert.AreEqual(AddSchoolProperty.Data.EnrollmentAmount, schoolPropertyData.EnrollmentAmount);

            var data = AddParentVMData();

            var AddParent = ParentService.AddNewParent(data);
            var studentData = AddStudentData();
            studentData.ParentId = 1;
            studentData.SectionId = AddSection.Data.Id;
            studentData.ClassId = 3;

            var AddStudent = await studentService.AddStudentToSchool(studentData);

            var result = await studentService.GetStudentById(AddStudent.Data.Id);

            Assert.That(!(result.ErrorMessages.Contains("Student does not exist")));
            Assert.AreEqual(studentData.ContactEmail, result.Data.EmailAddress);
        }

        [Test]
        public async Task GetStudentByID_Student_Does_Not_Exist_Test()
        {
            using ServicesDISetup _Setup = new ServicesDISetup();
            var studentService = _Setup.ServiceProvider.GetService<IStudentService>();
            var context = _Setup.ServiceProvider.GetService<AppDbContext>();
            var ParentService = _Setup.ServiceProvider.GetService<IParentService>();
            var SchoolPropertyService = _Setup.ServiceProvider.GetService<ISchoolPropertyService>();
            var ClassService = _Setup.ServiceProvider.GetService<ISchoolClassService>();
            var _sectionService = _Setup.ServiceProvider.GetService<ISectionService>();
            var _classArmService = _Setup.ServiceProvider.GetService<IClassArmService>();

            var GetAllParent = GetAllParentData();

            var SectionModel = AddClassSectionData();
            var ClassArmModel = AddClassArmData();
            var ClassModel = AddClassData();

            var school = AddSchoolData();
            context.Schools.Add(school);
            await context.SaveChangesAsync();

            var AddSection = await _sectionService.AddSection(SectionModel);
            var AddClassArm = await _classArmService.AddClassArm(ClassArmModel);

            ClassModel.SectionId = AddSection.Data.Id;

            List<long> ListOfClassArms = new List<long>()
            {
                AddClassArm.Data.Id
            };
            ClassModel.ClassArmIds = ListOfClassArms;

            var AddClass = await ClassService.AddClass(ClassModel);

            var schoolPropertyData = SchoolPropertyVMData();

            var AddSchoolProperty = await SchoolPropertyService.SetSchoolProperty(schoolPropertyData);
            Assert.AreEqual(AddSchoolProperty.Data.EnrollmentAmount, schoolPropertyData.EnrollmentAmount);

            var data = AddParentVMData();

            var AddParent = ParentService.AddNewParent(data);
            var studentData = AddStudentData();
            studentData.ParentId = 1;
            studentData.SectionId = AddSection.Data.Id;
            studentData.ClassId = 3;

            var AddStudent = await studentService.AddStudentToSchool(studentData);

            var result = await studentService.GetStudentById(AddStudent.Data.Id + 1);

            Assert.That(result.ErrorMessages.Contains("Student does not exist"));
        }

        [Test]
        public async Task GetStudentProfileById_Test()
        {
            using ServicesDISetup _Setup = new ServicesDISetup();
            var studentService = _Setup.ServiceProvider.GetService<IStudentService>();
            var context = _Setup.ServiceProvider.GetService<AppDbContext>();
            var ParentService = _Setup.ServiceProvider.GetService<IParentService>();
            var SchoolPropertyService = _Setup.ServiceProvider.GetService<ISchoolPropertyService>();
            var ClassService = _Setup.ServiceProvider.GetService<ISchoolClassService>();
            var _sectionService = _Setup.ServiceProvider.GetService<ISectionService>();
            var _classArmService = _Setup.ServiceProvider.GetService<IClassArmService>();
            var _AuthUserManagementService = _Setup.ServiceProvider.GetService<IAuthUserManagement>();
           
            var newAuthUserModel = AuthUserModelData();

            var GetAllParent = GetAllParentData();

            var SectionModel = AddClassSectionData();
            var ClassArmModel = AddClassArmData();
            var ClassModel = AddClassData();

            var school = AddSchoolData();
            context.Schools.Add(school);
            await context.SaveChangesAsync();

            var AddSection = await _sectionService.AddSection(SectionModel);
            var AddClassArm = await _classArmService.AddClassArm(ClassArmModel);
            var AddUser = await _AuthUserManagementService.AddUserAsync(newAuthUserModel);

            ClassModel.SectionId = AddSection.Data.Id;

            List<long> ListOfClassArms = new List<long>()
            {
                AddClassArm.Data.Id
            };
            ClassModel.ClassArmIds = ListOfClassArms;

            var AddClass = await ClassService.AddClass(ClassModel);

            var schoolPropertyData = SchoolPropertyVMData();

            var AddSchoolProperty = await SchoolPropertyService.SetSchoolProperty(schoolPropertyData);
            Assert.AreEqual(AddSchoolProperty.Data.EnrollmentAmount, schoolPropertyData.EnrollmentAmount);

            var data = AddParentVMData();

            var AddParent = ParentService.AddNewParent(data);
            var studentData = AddStudentData();
            studentData.ParentId = 1; 

            studentData.SectionId = AddSection.Data.Id;
            studentData.ClassId = 3;

            var AddStudent = await studentService.AddStudentToSchool(studentData);

            var result = await studentService.GetStudentProfileByUserId(AddStudent.Data.UserId.Value);

            Assert.That(!(result.ErrorMessages.Contains("Student does not exist")));
            Assert.AreEqual(studentData.ContactEmail, result.Data.EmailAddress);
        }

        [Test]
        public async Task GetStudentProfileById_Student_Not_Found_Test()
        {
            using ServicesDISetup _Setup = new ServicesDISetup();
            var studentService = _Setup.ServiceProvider.GetService<IStudentService>();
            var context = _Setup.ServiceProvider.GetService<AppDbContext>();
            var ParentService = _Setup.ServiceProvider.GetService<IParentService>();
            var SchoolPropertyService = _Setup.ServiceProvider.GetService<ISchoolPropertyService>();
            var ClassService = _Setup.ServiceProvider.GetService<ISchoolClassService>();
            var _sectionService = _Setup.ServiceProvider.GetService<ISectionService>();
            var _classArmService = _Setup.ServiceProvider.GetService<IClassArmService>();
            var _AuthUserManagementService = _Setup.ServiceProvider.GetService<IAuthUserManagement>();

            var newAuthUserModel = AuthUserModelData();

            var GetAllParent = GetAllParentData();

            var SectionModel = AddClassSectionData();
            var ClassArmModel = AddClassArmData();
            var ClassModel = AddClassData();

            var school = AddSchoolData();
            context.Schools.Add(school);
            await context.SaveChangesAsync();

            var AddSection = await _sectionService.AddSection(SectionModel);
            var AddClassArm = await _classArmService.AddClassArm(ClassArmModel);
            var AddUser = await _AuthUserManagementService.AddUserAsync(newAuthUserModel);

            ClassModel.SectionId = AddSection.Data.Id;

            List<long> ListOfClassArms = new List<long>()
            {
                AddClassArm.Data.Id
            };
            ClassModel.ClassArmIds = ListOfClassArms;

            var AddClass = await ClassService.AddClass(ClassModel);

            var schoolPropertyData = SchoolPropertyVMData();

            var AddSchoolProperty = await SchoolPropertyService.SetSchoolProperty(schoolPropertyData);
            Assert.AreEqual(AddSchoolProperty.Data.EnrollmentAmount, schoolPropertyData.EnrollmentAmount);

            var data = AddParentVMData();

            var AddParent = ParentService.AddNewParent(data);
            var studentData = AddStudentData();
            studentData.ParentId = 1;

            studentData.SectionId = AddSection.Data.Id;
            studentData.ClassId = 3;

            var AddStudent = await studentService.AddStudentToSchool(studentData);

            var newAlumni = new DeactivateStudent
            {
                SessionName = "session",
                DeactivationReason = "Promoted"
            };

            var removeStudent = await studentService.DeleteStudent(AddStudent.Data.Id, newAlumni);
            var result = await studentService.GetStudentProfileByUserId(AddStudent.Data.UserId.Value);

            Assert.That(result.ErrorMessages.Contains("Student does not exist"));
        }

        [Test]
        public async Task UpdateStudentTest()
        {
            using ServicesDISetup _setup = new ServicesDISetup();
            var studentService = _setup.ServiceProvider.GetService<IStudentService>();
            var ParentService = _setup.ServiceProvider.GetService<IParentService>();
            var SchoolPropertyService = _setup.ServiceProvider.GetService<ISchoolPropertyService>();
            var ClassService = _setup.ServiceProvider.GetService<ISchoolClassService>();
            var _sectionService = _setup.ServiceProvider.GetService<ISectionService>();
            var _classArmService = _setup.ServiceProvider.GetService<IClassArmService>();
            var _AuthService = _setup.ServiceProvider.GetService<IAuthUserManagement>();
            var context = _setup.ServiceProvider.GetService<AppDbContext>();

            var GetAllParent = GetAllParentData();
            var updateData = updateStudentData();
            var SectionModel = AddClassSectionData();
            var ClassArmModel = AddClassArmData();
            var ClassModel = AddClassData();

            var checkSchoolCount = _AuthService.GetAllAuthUsersAsync();

            var school = AddSchoolData();
            context.Schools.Add(school);
            await context.SaveChangesAsync();

            var AddSection = await _sectionService.AddSection(SectionModel);
            var AddClassArm = await _classArmService.AddClassArm(ClassArmModel);

            ClassModel.SectionId = AddSection.Data.Id;

            List<long> ListOfClassArms = new List<long>()
            {
                AddClassArm.Data.Id
            };
            ClassModel.ClassArmIds = ListOfClassArms;

            var AddClass = await ClassService.AddClass(ClassModel);

            var schoolPropertyData = SchoolPropertyVMData();

            var AddSchoolProperty = await SchoolPropertyService.SetSchoolProperty(schoolPropertyData);
            Assert.AreEqual(AddSchoolProperty.Data.EnrollmentAmount, schoolPropertyData.EnrollmentAmount);

            var data = AddParentVMData();

            var AddParent = ParentService.AddNewParent(data);
            var studentData = AddStudentData();
            studentData.ParentId = 1;
            studentData.SectionId = AddSection.Data.Id;
            studentData.ClassId = 3;

            var AddStudent = await studentService.AddStudentToSchool(studentData);
            var result = await studentService.UpdateStudent(AddStudent.Data.Id,updateData);

            Assert.That(result.ErrorMessages.Count == 0);
            Assert.AreEqual(updateData.ContactEmail,result.Data.Email);

        }

        [Test]
        public async Task UpdateStudent_Student_Does_Not_Exist_Test()
        {
            using ServicesDISetup _setup = new ServicesDISetup();
            var studentService = _setup.ServiceProvider.GetService<IStudentService>();
            var ParentService = _setup.ServiceProvider.GetService<IParentService>();
            var SchoolPropertyService = _setup.ServiceProvider.GetService<ISchoolPropertyService>();
            var ClassService = _setup.ServiceProvider.GetService<ISchoolClassService>();
            var _sectionService = _setup.ServiceProvider.GetService<ISectionService>();
            var _classArmService = _setup.ServiceProvider.GetService<IClassArmService>();
            var _AuthService = _setup.ServiceProvider.GetService<IAuthUserManagement>();
            var context = _setup.ServiceProvider.GetService<AppDbContext>();

            var GetAllParent = GetAllParentData();
            var updateData = updateStudentData();
            var SectionModel = AddClassSectionData();
            var ClassArmModel = AddClassArmData();
            var ClassModel = AddClassData();

            var checkSchoolCount = _AuthService.GetAllAuthUsersAsync();

            var school = AddSchoolData();
            context.Schools.Add(school);
            await context.SaveChangesAsync();

            var AddSection = await _sectionService.AddSection(SectionModel);
            var AddClassArm = await _classArmService.AddClassArm(ClassArmModel);

            ClassModel.SectionId = AddSection.Data.Id;

            List<long> ListOfClassArms = new List<long>()
            {
                AddClassArm.Data.Id
            };
            ClassModel.ClassArmIds = ListOfClassArms;

            var AddClass = await ClassService.AddClass(ClassModel);

            var schoolPropertyData = SchoolPropertyVMData();

            var AddSchoolProperty = await SchoolPropertyService.SetSchoolProperty(schoolPropertyData);
            Assert.AreEqual(AddSchoolProperty.Data.EnrollmentAmount, schoolPropertyData.EnrollmentAmount);

            var data = AddParentVMData();

            var AddParent = ParentService.AddNewParent(data);
            var studentData = AddStudentData();
            studentData.ParentId = 1;
            studentData.SectionId = AddSection.Data.Id;
            studentData.ClassId = 3;

            //Add student
            var AddStudent = await studentService.AddStudentToSchool(studentData);
            //remove student

            var newAlumni = new DeactivateStudent
            {
                SessionName = "session",
                DeactivationReason = "Promoted"
            };

            var removeStudent = await studentService.DeleteStudent(AddStudent.Data.Id, newAlumni);
            //Trying to update using the id of the deleted student
            var result = await studentService.UpdateStudent(AddStudent.Data.Id, updateData);

            Assert.That(result.ErrorMessages.Contains("Student does not exist"));
        }

        [Test]
        public async Task UpdateStudent_No_Parent_Exist_Test()
        {
            using ServicesDISetup _setup = new ServicesDISetup();
            var studentService = _setup.ServiceProvider.GetService<IStudentService>();
            var ParentService = _setup.ServiceProvider.GetService<IParentService>();
            var SchoolPropertyService = _setup.ServiceProvider.GetService<ISchoolPropertyService>();
            var ClassService = _setup.ServiceProvider.GetService<ISchoolClassService>();
            var _sectionService = _setup.ServiceProvider.GetService<ISectionService>();
            var _classArmService = _setup.ServiceProvider.GetService<IClassArmService>();
            var _AuthService = _setup.ServiceProvider.GetService<IAuthUserManagement>();
            var context = _setup.ServiceProvider.GetService<AppDbContext>();

            var GetAllParent = GetAllParentData();
            var updateData = second_UpdateStudentData();
            var SectionModel = AddClassSectionData();
            var ClassArmModel = AddClassArmData();
            var ClassModel = AddClassData();

            var checkSchoolCount = _AuthService.GetAllAuthUsersAsync();

            var school = AddSchoolData();
            context.Schools.Add(school);
            await context.SaveChangesAsync();

            var AddSection = await _sectionService.AddSection(SectionModel);
            var AddClassArm = await _classArmService.AddClassArm(ClassArmModel);

            ClassModel.SectionId = AddSection.Data.Id;

            List<long> ListOfClassArms = new List<long>()
            {
                AddClassArm.Data.Id
            };
            ClassModel.ClassArmIds = ListOfClassArms;

            var AddClass = await ClassService.AddClass(ClassModel);

            var schoolPropertyData = SchoolPropertyVMData();

            var AddSchoolProperty = await SchoolPropertyService.SetSchoolProperty(schoolPropertyData);
            Assert.AreEqual(AddSchoolProperty.Data.EnrollmentAmount, schoolPropertyData.EnrollmentAmount);

            var data = AddParentVMData();

            var AddParent = ParentService.AddNewParent(data);
            var studentData = AddStudentData();
            studentData.ParentId = 1;
            studentData.SectionId = AddSection.Data.Id;
            studentData.ClassId = 3;
            updateData.ClassId = 3;

            var AddStudent = await studentService.AddStudentToSchool(studentData);
            var result = await studentService.UpdateStudent(AddStudent.Data.Id, updateData);

            Assert.That(result.ErrorMessages.Contains("No parent exists"));
        }

        [Test]
        public async Task UpdateStudent_No_Class_Exist_Test()
        {
            using ServicesDISetup _setup = new ServicesDISetup();
            var studentService = _setup.ServiceProvider.GetService<IStudentService>();
            var ParentService = _setup.ServiceProvider.GetService<IParentService>();
            var SchoolPropertyService = _setup.ServiceProvider.GetService<ISchoolPropertyService>();
            var ClassService = _setup.ServiceProvider.GetService<ISchoolClassService>();
            var _sectionService = _setup.ServiceProvider.GetService<ISectionService>();
            var _classArmService = _setup.ServiceProvider.GetService<IClassArmService>();
            var _AuthService = _setup.ServiceProvider.GetService<IAuthUserManagement>();
            var context = _setup.ServiceProvider.GetService<AppDbContext>();

            var GetAllParent = GetAllParentData();
            var updateData = second_UpdateStudentData();
            var SectionModel = AddClassSectionData();
            var ClassArmModel = AddClassArmData();
            var ClassModel = AddClassData();

            var checkSchoolCount = _AuthService.GetAllAuthUsersAsync();

            var school = AddSchoolData();
            context.Schools.Add(school);
            await context.SaveChangesAsync();

            var AddSection = await _sectionService.AddSection(SectionModel);
            var AddClassArm = await _classArmService.AddClassArm(ClassArmModel);

            ClassModel.SectionId = AddSection.Data.Id;

            List<long> ListOfClassArms = new List<long>()
            {
                AddClassArm.Data.Id
            };
            ClassModel.ClassArmIds = ListOfClassArms;

            var AddClass = await ClassService.AddClass(ClassModel);

            var schoolPropertyData = SchoolPropertyVMData();

            var AddSchoolProperty = await SchoolPropertyService.SetSchoolProperty(schoolPropertyData);
            Assert.AreEqual(AddSchoolProperty.Data.EnrollmentAmount, schoolPropertyData.EnrollmentAmount);

            var data = AddParentVMData();

            var AddParent = ParentService.AddNewParent(data);
            var studentData = AddStudentData();
            studentData.ParentId = 1;
            studentData.SectionId = AddSection.Data.Id;
            studentData.ClassId = 3;
            updateData.ParentId = 1;

            var AddStudent = await studentService.AddStudentToSchool(studentData);
            var result = await studentService.UpdateStudent(AddStudent.Data.Id, updateData);

            Assert.That(result.ErrorMessages.Contains("class does not exists"));
        }





        private CreateStudentVM AddStudentData()
        {
            List<DocumentType> DocumentTypes = new List<DocumentType>() { DocumentType.Logo, DocumentType.ProfilePhoto };
            var data = new CreateStudentVM()
            {
                FirstName = "Simi",
                LastName = "Segun",
                Sex = "Female",
                Religion = "christian",
                Nationality = "Nigeria",
                ClassId = 1,
                StudentType = StudentType.DayStudent,
                DocumentTypes = DocumentTypes,
                ContactEmail = "SegunSi@yahoo.com",
                ContactPhone = "09023217867",
                Allergies = "none",
                BloodGroup = "O+",
                Disability = false,
                DateOfBirth = new DateTime(2001,08,12)
            };

            return data;
        }
        private AddParentVM AddParentVMData()
        {
            var data = new AddParentVM()
            {
                Title = "Mr",
                FirstName = "Oke",
                LastName = "Ekene",
                Sex = "Male",
                Occupation = "Doctor",
                ModeOfIdentification = "NIN",
                IdentificationNumber = "9788892718",
                PhoneNumber = "09087675645",
                EmailAddress = "bued@gmail.com",
                HomeAddress = "7 kunle lawal",
                OfficeAddress = "17h karimu kolawate",
                DocumentType = DocumentType.Logo
            };

            return data;
        }
        private SchoolPropertyVM SchoolPropertyVMData()
        {
            var data = new SchoolPropertyVM()
            {
                Prefix = "GA",
                Seperator = ":",
                EnrollmentAmount = 100000,
                NumberOfTerms = 3,
                ClassDays = ClassDaysType.WeekDaysOnly
            };
            return data;
        }

        private QueryModel GetAllParentData()
        {
            var data = new QueryModel()
            {
                PageIndex = 1,
                PageSize = 10
            };
            return data;
        }

        private School AddSchoolData()
        {
            var listOfConttactDetails = new List<SchoolContactDetails>();
            var contactDetails = new SchoolContactDetails()
            {
                Email = "dabby@yopmail.com",
                EmailPassword = "Osabuede",
                PhoneNumber = "09089786756",
                FirstName = "Test",
                LastName = "Tested"          
            };

            listOfConttactDetails.Add(contactDetails);

            var school = new School()
            {
                Id = 4,
                DomainName = "Test",
                Name = "Test School",
                SchoolContactDetails = listOfConttactDetails,
                Address = "No 8 kuolmi close",
                City =  "Ajah",
                Country =  "Nigeria",
                IsActive = true,
                PrimaryColor = "#000000"
            };

            return school;
        }
        private ClassSectionVM AddClassSectionData()
        {
            var model = new ClassSectionVM()
            {
                Name = "Testing",
            };

            return model;
        }
        private AddClassArm AddClassArmData()
        {
            var data = new AddClassArm()
            {
                Name = "YellowLabel",
                Status = true
            };
            return data;
        }
        private AddClassVM AddClassData()
        {
            var data = new AddClassVM()
            {
                Name = "Sanyu kol",
                IsTerminalClass = false,
                Sequence = 3,
                Status = true
            };
            return data;
        }

        private QueryModel QueryModelData()
        {
            var data = new QueryModel()
            {
                PageIndex = 1,
                PageSize = 10
            };
            return data;
        }
        private AuthUserModel AuthUserModelData()
        {
            var newAuthUserModel = new AuthUserModel()
            {
                FirstName = "Ade",
                LastName = "Ola",
                Email = "AdeOla@gmail.com",
                PhoneNumber = "09089787632",
                Password = "GabbySTeams1990",
                UserType = UserType.Student
            };

            return newAuthUserModel;
        }

        private StudentUpdateVM updateStudentData()
        {
            List<DocumentType> DocumentTypes = new List<DocumentType>() { DocumentType.Logo, DocumentType.ProfilePhoto };
            var data = new StudentUpdateVM()
            {
                FirstName = "UpdatedSimi",
                LastName = "UpdatedSegun",
                Sex = "male",
                Religion = "christian",
                Nationality = "Nigeria",
                ClassId = 3,
                ParentId = 1,
                StudentType = StudentType.DayStudent,
                DocumentTypes = DocumentTypes,
                ContactEmail = "updatedSegunSi@yahoo.com",
                ContactPhone = "09023217867",
                Allergies = "none",
                BloodGroup = "O+",
                Disability = false,
                DateOfBirth = new DateTime(2001, 08, 12)
            };

            return data;
        }

        private StudentUpdateVM second_UpdateStudentData()
        {
            List<DocumentType> DocumentTypes = new List<DocumentType>() { DocumentType.Logo, DocumentType.ProfilePhoto };
            var data = new StudentUpdateVM()
            {
                FirstName = "UpdatedSimi",
                LastName = "UpdatedSegun",
                Sex = "male",
                Religion = "christian",
                Nationality = "Nigeria",
                StudentType = StudentType.DayStudent,
                DocumentTypes = DocumentTypes,
                ContactEmail = "updatedSegunSi@yahoo.com",
                ContactPhone = "09023217867",
                Allergies = "none",
                BloodGroup = "O+",
                Disability = false,
                DateOfBirth = new DateTime(2001, 08, 12)
            };

            return data;
        }
        

    
    }
}
