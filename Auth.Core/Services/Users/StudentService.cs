﻿using Microsoft.EntityFrameworkCore;
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
using Auth.Core.ViewModels.Student;
using Shared.Entities;
using Shared.Utils;
using Microsoft.AspNetCore.Identity;
using Auth.Core.Context;
using Auth.Core.ViewModels;

namespace Auth.Core.Services
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IAuthUserManagement _authUserManagement;

        private readonly IRepository<Student, long> _studentRepo;
        private readonly IUnitOfWork _unitOfWork;

        public StudentService(IRepository<Student, long> studentRepo, IUnitOfWork unitOfWork, IAuthUserManagement authUserManagement, AppDbContext appDbContext)
        {
            _studentRepo = studentRepo;
            _unitOfWork = unitOfWork;
            _authUserManagement = authUserManagement;
            _appDbContext = appDbContext;
        }

        public async Task<ResultModel<StudentVM>> AddStudentToSchool(StudentVM model)
        {
            var result = new ResultModel<StudentVM>();

            //create auth user
            var userModel = new AuthUserModel { FirstName = model.FirstName, LastName = model.LastName, Email = model.Email, Password = model.Password, PhoneNumber = model.PhoneNumber };
            var authResult = await _authUserManagement.AddUserAsync(userModel);

            if (authResult == null)
            {
                result.AddError("Failed to add authentication for student");
                return result;
            }

            var stud = _studentRepo.Insert(new Student { UserId = authResult.Value });
            await _unitOfWork.SaveChangesAsync();
            model.Id = stud.Id;
            model.Id = stud.Id;
            result.Data = model;
            return result;
        }

        public async Task<ResultModel<bool>> DeleteStudent(long Id)
        {
            var result = new ResultModel<bool> { Data = false };

            //check if the student exists
            var std = await _studentRepo.FirstOrDefaultAsync(Id);
            if (std == null)
            {
                result.AddError("Student does not exist");
                return result;
            }

            //delete auth user
            var authResult = await _authUserManagement.DeleteUserAsync(std.UserId);

            if (authResult == false)
            {
                result.AddError("Failed to delete authentication for student");
                return result;
            }

            await _studentRepo.DeleteAsync(Id);
            await _unitOfWork.SaveChangesAsync();
            result.Data = true;

            return result;
        }

        public async Task<ResultModel<List<StudentVM>>> GetAllStudentsInSchool(int pageNumber, int pageSize)
        {
            //use appdbcontext directly so that we can do a join with the auth users table
            var query = _appDbContext.Students.Join(
                _appDbContext.Users, student => student.UserId, authUser => authUser.Id,
                (student, authUser) => new StudentVM
                {
                    FirstName = authUser.FirstName,
                    LastName = authUser.LastName,
                    Email = authUser.Email,
                    PhoneNumber = authUser.PhoneNumber
                });

            var pagedData = await PaginatedList<StudentVM>.CreateAsync(query, pageNumber, pageSize);

            var result = new ResultModel<List<StudentVM>>
            {
                Data = pagedData
            };
            return result;
        }

        public async Task<ResultModel<StudentVM>> GetStudentById(long Id)
        {
            var result = new ResultModel<StudentVM>();
            var std = await _studentRepo.FirstOrDefaultAsync(x => x.Id == Id);

            if (std == null)
            {
                result.AddError("Student does not exist");
                return result;
            }

            result.Data = std;
            return result;
        }

        public async Task<ResultModel<StudentUpdateVM>> UpdateStudent(StudentUpdateVM model)
        {
            var std = await _studentRepo.FirstOrDefaultAsync(model.Id);
            var result = new ResultModel<StudentUpdateVM>();

            if (std == null)
            {
                result.AddError("Student does not exist");
                return result;
            }

            //update auth user
            var userModel = new AuthUserModel { FirstName = model.FirstName, LastName = model.LastName };
            var authResult = await _authUserManagement.UpdateUserAsync(std.UserId, userModel);

            if (authResult == false)
            {
                result.AddError("Failed to update authentication model for student");
                return result;
            }

            //TODO: add more props

            await _studentRepo.UpdateAsync(std);
            await _unitOfWork.SaveChangesAsync();
            result.Data = model;
            return result;
        }
    }
}