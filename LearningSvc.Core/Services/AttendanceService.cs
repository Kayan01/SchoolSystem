using LearningSvc.Core.Interfaces;
using LearningSvc.Core.Models;
using LearningSvc.Core.ViewModels.Attendance;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningSvc.Core.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IRepository<AttendanceSubject, long> _subjectAttendanceRepo;
        private readonly IRepository<AttendanceClass, long> _classAttendanceRepo;
        private readonly IUnitOfWork _unitOfWork;

        public AttendanceService(
            IRepository<AttendanceSubject, long> subjectAttendanceRepo, 
            IRepository<AttendanceClass, long> classAttendanceRepo,
            IUnitOfWork unitOfWork
            )
        {
            _classAttendanceRepo = classAttendanceRepo;
            _subjectAttendanceRepo = subjectAttendanceRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultModel<string>> AddAttendanceForClass(AddClassAttendanceVM model)
        {
            var currAttendance = await _classAttendanceRepo.GetAll()
                .Where(x => x.AttendanceDate == model.Date && x.ClassId == model.ClassId)
                .ToListAsync();

            //update existing attendance
            if (currAttendance.Count > 0)
            {

                foreach (var item in model.StudentAttendanceVMs)
                {
                    var currAtt = currAttendance.FirstOrDefault(x => x.StudentId == item.StudentId);

                    if (currAtt != null)
                    {
                        _classAttendanceRepo.Insert(new AttendanceClass
                        {
                            Id = currAtt.Id,
                            AttendanceDate = model.Date,
                            AttendanceStatus = item.AttendanceStatus,
                            ClassId = model.ClassId,
                            StudentId = currAtt.StudentId

                        });
                    }
                    else
                    {
                        _classAttendanceRepo.Insert(new AttendanceClass
                        {
                            AttendanceDate = model.Date,
                            AttendanceStatus = item.AttendanceStatus,
                            ClassId = model.ClassId,
                            StudentId = item.StudentId
                        });
                    }
                }
            }
            else
            {
                foreach (var item in model.StudentAttendanceVMs)
                {
                       _classAttendanceRepo.Insert(new AttendanceClass
                        {
                            AttendanceDate = model.Date,
                            AttendanceStatus = item.AttendanceStatus,
                            ClassId = model.ClassId,
                            StudentId = item.StudentId
                        });
                    
                }
            }

           await _unitOfWork.SaveChangesAsync();

            return new ResultModel<string>(data : "Attendance saved");   
        }

        public async  Task<ResultModel<string>> AddAttendanceForSubject(AddSubjectAttendanceVM model)
        {
            var currAttendance = await _subjectAttendanceRepo.GetAll()
                  .Where(x => x.AttendanceDate == model.Date && x.SubjectId == model.SubjectId)
                  .ToListAsync();

            //update existing attendance
            if (currAttendance.Count > 0)
            {

                foreach (var item in model.StudentAttendanceVMs)
                {
                    var currAtt = currAttendance.FirstOrDefault(x => x.StudentId == item.StudentId);

                    if (currAtt != null)
                    {
                        _subjectAttendanceRepo.Insert(new AttendanceSubject
                        {
                            Id = currAtt.Id,
                            AttendanceDate = model.Date,
                            AttendanceStatus = item.AttendanceStatus,
                            SubjectId = model.SubjectId,
                            StudentId = currAtt.StudentId

                        });
                    }
                    else
                    {
                        _subjectAttendanceRepo.Insert(new AttendanceSubject
                        {
                            AttendanceDate = model.Date,
                            AttendanceStatus = item.AttendanceStatus,
                            SubjectId = model.SubjectId,
                            StudentId = item.StudentId
                        });
                    }
                }
            }
            else
            {
                foreach (var item in model.StudentAttendanceVMs)
                {
                    _subjectAttendanceRepo.Insert(new AttendanceSubject
                    {
                        AttendanceDate = model.Date,
                        AttendanceStatus = item.AttendanceStatus,
                        SubjectId = model.SubjectId,
                        StudentId = item.StudentId
                    });

                }
            }

            await _unitOfWork.SaveChangesAsync();

            return new ResultModel<string>(data: "Attendance saved");
        }
    }
}
