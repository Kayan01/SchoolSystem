using LearningSvc.Core.Enumerations;
using LearningSvc.Core.Models.TimeTable;
using LearningSvc.Core.Interfaces;
using LearningSvc.Core.ViewModels.TimeTable;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace LearningSvc.Core.Services
{
    public class TimeTableService : ITimeTableService
    {
        private readonly IRepository<TimeTableCell, long> _timeTableRepo;
        private readonly IRepository<Period, long> _periodRepo;
        private readonly IUnitOfWork _unitOfWork;

        public TimeTableService(IUnitOfWork unitOfWork, IRepository<TimeTableCell, long> timeTableRepo, IRepository<Period, long> periodRepo)
        {
            _unitOfWork = unitOfWork;
            _timeTableRepo = timeTableRepo;
            _periodRepo = periodRepo;
        }

        public async Task<ResultModel<List<ClassSessionOutputVM>>> GetAllClassesForTeacherToday(long teacherId, WeekDays day)
        {
            var result = new ResultModel<List<ClassSessionOutputVM>>
            {
                Data = await _timeTableRepo.GetAll().Where(m => m.TeacherClassSubject.TeacherId == teacherId && m.Day == day)
                .Select(x => new ClassSessionOutputVM
                {
                    TimeTableCellId = x.Id,
                    ClassName = x.TeacherClassSubject.SchoolClassSubject.SchoolClass.Name,
                    SubjectName = x.TeacherClassSubject.SchoolClassSubject.Subject.Name,
                    TeacherName = $"{x.TeacherClassSubject.Teacher.FirstName} {x.TeacherClassSubject.Teacher.LastName}",
                    TimeFrom = x.Period.TimeFrom,
                    TimeTo = x.Period.TimeTo,
                    NoOfPeriods = x.NoOfPeriod,
                    PeriodName = x.Period.Name,
                    NoOfStudent = x.TeacherClassSubject.SchoolClassSubject.SchoolClass.Students.Count,
                    HasVirtual = x.HasVirtual,
                    ZoomId = x.TeacherClassSubject.SchoolClassSubject.SchoolClass.ZoomRoomId
                }).ToListAsync()
            };
            return result;
        }

        public async Task<ResultModel<List<PeriodVM>>> GetAllPeriodForSchool()
        {
            var result = new ResultModel<List<PeriodVM>>
            {
                Data = await _periodRepo.GetAll().Select(x => (PeriodVM)x).ToListAsync()
            };
            return result;
        }

        public async Task<ResultModel<List<ClassSessionOutputVM>>> GetNextClassesForTeacherToday(long teacherId, WeekDays day, int curPeriod, int Count)
        {
            var result = new ResultModel<List<ClassSessionOutputVM>>
            {
                Data = await _timeTableRepo.GetAll().Where(m => m.TeacherClassSubject.TeacherId == teacherId && m.Day == day && m.Period.Step > curPeriod)
                    .Select(x => new ClassSessionOutputVM
                    {
                        TimeTableCellId = x.Id,
                        ClassName = x.TeacherClassSubject.SchoolClassSubject.SchoolClass.Name,
                        SubjectName = x.TeacherClassSubject.SchoolClassSubject.Subject.Name,
                        TeacherName = $"{x.TeacherClassSubject.Teacher.FirstName} {x.TeacherClassSubject.Teacher.LastName}",
                        TimeFrom = x.Period.TimeFrom,
                        TimeTo = x.Period.TimeTo,
                        NoOfPeriods = x.NoOfPeriod,
                        PeriodName = x.Period.Name,
                        NoOfStudent = x.TeacherClassSubject.SchoolClassSubject.SchoolClass.Students.Count,
                        HasVirtual = x.HasVirtual,
                        ZoomId = x.TeacherClassSubject.SchoolClassSubject.SchoolClass.ZoomRoomId
                    })
                    .Take(Count)
                    .ToListAsync()
            };
            return result;
        }

        public async Task<ResultModel<List<TimeTableCellVM>>> GetTimeTableCellsForClass(long classId)
        {
            var result = new ResultModel<List<TimeTableCellVM>>
            {
                Data = await _timeTableRepo.GetAll().Where(m => m.TeacherClassSubject.SchoolClassSubject.SchoolClassId == classId)
                    .Select(x => new TimeTableCellVM
                    {
                        Id = x.Id,
                        PeriodId = x.PeriodId,
                        PeriodName = x.Period.Name,
                        Day = x.Day,
                        TeacherClassSubjectId = x.TeacherClassSubjectId,
                        TeacherId = x.TeacherClassSubject.TeacherId,
                        TeacherName = $"{x.TeacherClassSubject.Teacher.FirstName} {x.TeacherClassSubject.Teacher.LastName}",
                        SubjectId = x.TeacherClassSubject.SchoolClassSubject.SubjectId,
                        SubjectName = x.TeacherClassSubject.SchoolClassSubject.Subject.Name,
                        SchoolClassId = x.TeacherClassSubject.SchoolClassSubject.SchoolClassId,
                        ClassName = x.TeacherClassSubject.SchoolClassSubject.SchoolClass.Name,
                        NoOfPeriod = x.NoOfPeriod,
                        HasVirtual = x.HasVirtual,
                        ZoomId = x.TeacherClassSubject.SchoolClassSubject.SchoolClass.ZoomRoomId,

                    }).ToListAsync()
            };
            return result;
        }

        public async Task<ResultModel<List<TimeTableCellVM>>> GetTimeTableCellsForTeacher(long teacherId)
        {
            var result = new ResultModel<List<TimeTableCellVM>>
            {
                Data = await _timeTableRepo.GetAll().Where(m => m.TeacherClassSubject.TeacherId == teacherId)
                    .Select(x => new TimeTableCellVM
                    {
                        Id = x.Id,
                        PeriodId = x.PeriodId,
                        PeriodName = x.Period.Name,
                        Day = x.Day,
                        TeacherClassSubjectId = x.TeacherClassSubjectId,
                        TeacherId = x.TeacherClassSubject.TeacherId,
                        TeacherName = $"{x.TeacherClassSubject.Teacher.FirstName} {x.TeacherClassSubject.Teacher.LastName}",
                        SubjectId = x.TeacherClassSubject.SchoolClassSubject.SubjectId,
                        SubjectName = x.TeacherClassSubject.SchoolClassSubject.Subject.Name,
                        SchoolClassId = x.TeacherClassSubject.SchoolClassSubject.SchoolClassId,
                        ClassName = x.TeacherClassSubject.SchoolClassSubject.SchoolClass.Name,
                        NoOfPeriod = x.NoOfPeriod,
                        HasVirtual = x.HasVirtual,
                        ZoomId = x.TeacherClassSubject.SchoolClassSubject.SchoolClass.ZoomRoomId,

                    }).ToListAsync()
            };
            return result;
        }

        public async Task<ResultModel<List<PeriodVM>>> SetupSchoolPeriods(List<PeriodInsertVM> model)
        {
            var result = new ResultModel<List<PeriodVM>>();

            var oldPeriod = await _periodRepo.GetAll().ToListAsync();

            if (oldPeriod.Count > 0)
            {
                //delete all timetable for a school when period is updated
                await _timeTableRepo.DeleteAsync(m=>m.TenantId == oldPeriod[0].TenantId);

                //delete all periods for a school so that new once can be created.
                await _periodRepo.DeleteAsync(m => m.TenantId == oldPeriod[0].TenantId);
            }

            result.Data = new List<PeriodVM>();

            foreach (var item in model)
            {
                var p = new Period
                {
                    Name = item.Name,
                    Step = item.Step,
                    TimeFrom = TimeSpan.ParseExact(item.TimeFrom, "h\\:mm", CultureInfo.CurrentCulture),
                    TimeTo = TimeSpan.ParseExact(item.TimeTo, "h\\:mm", CultureInfo.CurrentCulture),
                    IsBreak = item.isBreak
                };

                await _periodRepo.InsertAsync(p);

                result.Data.Add(p);
            }

            await _unitOfWork.SaveChangesAsync();
            
            return result;
        }

        public async Task<ResultModel<TimeTableCellVM>> AddTimeTableCell(TimeTableCellInsertVM model)
        {
            var result = new ResultModel<TimeTableCellVM>();
            
            var t = new TimeTableCell
            {
                Day = model.Day,
                HasVirtual = model.HasVirtual,
                NoOfPeriod = model.NoOfPeriod,
                PeriodId = model.PeriodId,
                TeacherClassSubjectId = model.TeacherClassSubjectId,
            };

            var id = await _timeTableRepo.InsertAndGetIdAsync(t);

            await _unitOfWork.SaveChangesAsync();

            result.Data = await _timeTableRepo.GetAll().Where(m => m.Id == id)
                .Select(x => new TimeTableCellVM
                {
                    Id = x.Id,
                    PeriodId = x.PeriodId,
                    PeriodName = x.Period.Name,
                    Day = x.Day,
                    TeacherClassSubjectId = x.TeacherClassSubjectId,
                    TeacherId = x.TeacherClassSubject.TeacherId,
                    TeacherName = $"{x.TeacherClassSubject.Teacher.FirstName} {x.TeacherClassSubject.Teacher.LastName}",
                    SubjectId = x.TeacherClassSubject.SchoolClassSubject.SubjectId,
                    SubjectName = x.TeacherClassSubject.SchoolClassSubject.Subject.Name,
                    SchoolClassId = x.TeacherClassSubject.SchoolClassSubject.SchoolClassId,
                    ClassName = x.TeacherClassSubject.SchoolClassSubject.SchoolClass.Name,
                    NoOfPeriod = x.NoOfPeriod,
                    HasVirtual = x.HasVirtual,
                    ZoomId = x.TeacherClassSubject.SchoolClassSubject.SchoolClass.ZoomRoomId,

                }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<ResultModel<string>> DeleteTimeTableCell(long TimeTableCellId)
        {
            var result = new ResultModel<string>();

            var timeTableCell = await _timeTableRepo.GetAsync(TimeTableCellId);

            if (timeTableCell == null)
            {
                result.AddError("Time Table Cell was not found");
                return result;
            }

            await _timeTableRepo.DeleteAsync(timeTableCell);
            await _unitOfWork.SaveChangesAsync();

            result.Data = "Deleted Successfully";
            return result;
        }
    }
}
