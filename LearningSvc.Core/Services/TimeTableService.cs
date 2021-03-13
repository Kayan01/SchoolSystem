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
using LearningSvc.Core.Models;

namespace LearningSvc.Core.Services
{
    public class TimeTableService : ITimeTableService
    {
        private readonly IRepository<TimeTableCell, long> _timeTableRepo;
        private readonly IRepository<Period, long> _periodRepo;
        private readonly IRepository<TeacherClassSubject, long> _teacherClassSubjectRepo;
        private readonly IRepository<Teacher, long> _teacherRepo;
        private readonly IUnitOfWork _unitOfWork;

        public TimeTableService(IUnitOfWork unitOfWork, 
            IRepository<TimeTableCell, long> timeTableRepo, 
            IRepository<Period, long> periodRepo, 
            IRepository<TeacherClassSubject, long> teacherClassSubjectRepo,
            IRepository<Teacher, long> teacherRepo)
        {
            _unitOfWork = unitOfWork;
            _timeTableRepo = timeTableRepo;
            _periodRepo = periodRepo;
            _teacherClassSubjectRepo = teacherClassSubjectRepo;
            _teacherRepo = teacherRepo;
        }

        public async Task<ResultModel<List<ClassSessionOutputVM>>> GetAllClassesForTeacherToday(long currentUserId, WeekDays day)
        {
            var result = new ResultModel<List<ClassSessionOutputVM>>();

            var teacher = await _teacherRepo.GetAll().Where(m => m.UserId == currentUserId).FirstOrDefaultAsync();
            if (teacher == null)
            {
                result.AddError("Current user is not a valid Teacher");
                return result;
            }

            result.Data = await _timeTableRepo.GetAll().Where(m => m.TeacherClassSubject.TeacherId == teacher.Id && m.Day == day)
                .Select(x => new ClassSessionOutputVM
                {
                    TimeTableCellId = x.Id,
                    ClassName = $"{x.TeacherClassSubject.SchoolClassSubject.SchoolClass.Name} {x.TeacherClassSubject.SchoolClassSubject.SchoolClass.ClassArm}",
                    SubjectName = x.TeacherClassSubject.SchoolClassSubject.Subject.Name,
                    TeacherName = $"{x.TeacherClassSubject.Teacher.FirstName} {x.TeacherClassSubject.Teacher.LastName}",
                    TimeFrom = x.Period.TimeFrom,
                    TimeTo = x.Period.TimeTo,
                    NoOfPeriods = x.NoOfPeriod,
                    PeriodName = x.Period.Name,
                    NoOfStudent = x.TeacherClassSubject.SchoolClassSubject.SchoolClass.Students.Count,
                    HasVirtual = x.HasVirtual,
                    ZoomId = x.TeacherClassSubject.SchoolClassSubject.SchoolClass.ZoomRoomId,
                    ZoomStartUrl= x.TeacherClassSubject.SchoolClassSubject.SchoolClass.ZoomRoomStartUrl,
                }).ToListAsync();

            return result;
        }

        public async Task<ResultModel<List<ClassSessionOutputVM>>> GetNextClassesForTeacherToday(long currentUserId, WeekDays day, int curPeriod, int Count)
        {
            var result = new ResultModel<List<ClassSessionOutputVM>>();

            var teacher = await _teacherRepo.GetAll().Where(m => m.UserId == currentUserId).FirstOrDefaultAsync();
            if (teacher == null)
            {
                result.AddError("Current user is not a valid Teacher");
                return result;
            }

            result.Data = await _timeTableRepo.GetAll().Where(m => m.TeacherClassSubject.TeacherId == teacher.Id && m.Day == day && m.Period.Step > curPeriod)
                .Select(x => new ClassSessionOutputVM
                {
                    TimeTableCellId = x.Id,
                    ClassName = $"{x.TeacherClassSubject.SchoolClassSubject.SchoolClass.Name} {x.TeacherClassSubject.SchoolClassSubject.SchoolClass.ClassArm}",
                    SubjectName = x.TeacherClassSubject.SchoolClassSubject.Subject.Name,
                    TeacherName = $"{x.TeacherClassSubject.Teacher.FirstName} {x.TeacherClassSubject.Teacher.LastName}",
                    TimeFrom = x.Period.TimeFrom,
                    TimeTo = x.Period.TimeTo,
                    NoOfPeriods = x.NoOfPeriod,
                    PeriodName = x.Period.Name,
                    NoOfStudent = x.TeacherClassSubject.SchoolClassSubject.SchoolClass.Students.Count,
                    HasVirtual = x.HasVirtual,
                    ZoomId = x.TeacherClassSubject.SchoolClassSubject.SchoolClass.ZoomRoomId,
                    ZoomStartUrl = x.TeacherClassSubject.SchoolClassSubject.SchoolClass.ZoomRoomStartUrl,
                })
                .Take(Count)
                .ToListAsync();

            return result;
        }

        public async Task<ResultModel<List<ClassSessionOutputVM>>> GetAllClassesForClassToday(long classId, WeekDays day)
        {
            var result = new ResultModel<List<ClassSessionOutputVM>>();

            result.Data = await _timeTableRepo.GetAll().Where(m => m.TeacherClassSubject.SchoolClassSubject.SchoolClassId == classId && m.Day == day)
                .Select(x => new ClassSessionOutputVM
                {
                    TimeTableCellId = x.Id,
                    ClassName = $"{x.TeacherClassSubject.SchoolClassSubject.SchoolClass.Name} {x.TeacherClassSubject.SchoolClassSubject.SchoolClass.ClassArm}",
                    SubjectName = x.TeacherClassSubject.SchoolClassSubject.Subject.Name,
                    TeacherName = $"{x.TeacherClassSubject.Teacher.FirstName} {x.TeacherClassSubject.Teacher.LastName}",
                    TimeFrom = x.Period.TimeFrom,
                    TimeTo = x.Period.TimeTo,
                    NoOfPeriods = x.NoOfPeriod,
                    PeriodName = x.Period.Name,
                    NoOfStudent = x.TeacherClassSubject.SchoolClassSubject.SchoolClass.Students.Count,
                    HasVirtual = x.HasVirtual,
                    ZoomId = x.TeacherClassSubject.SchoolClassSubject.SchoolClass.ZoomRoomId
                }).ToListAsync();

            return result;
        }

        public async Task<ResultModel<List<ClassSessionOutputVM>>> GetNextClassesForClassToday(long classId, WeekDays day, int curPeriod, int Count)
        {
            var result = new ResultModel<List<ClassSessionOutputVM>>();

            result.Data = await _timeTableRepo.GetAll().Where(m => m.TeacherClassSubject.SchoolClassSubject.SchoolClassId == classId && m.Day == day && m.Period.Step > curPeriod)
                .Select(x => new ClassSessionOutputVM
                {
                    TimeTableCellId = x.Id,
                    ClassName = $"{x.TeacherClassSubject.SchoolClassSubject.SchoolClass.Name} {x.TeacherClassSubject.SchoolClassSubject.SchoolClass.ClassArm}",
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
                .ToListAsync();

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

        public async Task<ResultModel<List<TimeTableCellVM>>> GetTimeTableCellsForClass( long classId)
        {
            var result = new ResultModel<List<TimeTableCellVM>>();

            result = new ResultModel<List<TimeTableCellVM>>
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
                        ClassName = $"{x.TeacherClassSubject.SchoolClassSubject.SchoolClass.Name} {x.TeacherClassSubject.SchoolClassSubject.SchoolClass.ClassArm}",
                        NoOfPeriod = x.NoOfPeriod,
                        HasVirtual = x.HasVirtual,
                        ZoomId = x.TeacherClassSubject.SchoolClassSubject.SchoolClass.ZoomRoomId,

                    }).ToListAsync()
            };

            return result;
        }

        public async Task<ResultModel<List<TimeTableCellVM>>> GetTimeTableCellsForTeacher(long currentUserId)
        {
            var result = new ResultModel<List<TimeTableCellVM>>();

            var teacher = await _teacherRepo.GetAll().Where(m => m.UserId == currentUserId).FirstOrDefaultAsync();
            if (teacher == null)
            {
                result.AddError("Current user is not a valid Teacher");
                return result;
            }

             result.Data = await _timeTableRepo.GetAll().Where(m => m.TeacherClassSubject.TeacherId == teacher.Id)
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
                    ClassName = $"{x.TeacherClassSubject.SchoolClassSubject.SchoolClass.Name} {x.TeacherClassSubject.SchoolClassSubject.SchoolClass.ClassArm}",
                    NoOfPeriod = x.NoOfPeriod,
                    HasVirtual = x.HasVirtual,
                    ZoomId = x.TeacherClassSubject.SchoolClassSubject.SchoolClass.ZoomRoomId,
                    ZoomStartUrl = x.TeacherClassSubject.SchoolClassSubject.SchoolClass.ZoomRoomStartUrl,

                }).ToListAsync();

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

            var checkTeacherClassSubjectExists = await _teacherClassSubjectRepo.FirstOrDefaultAsync(model.TeacherClassSubjectId);
            if (checkTeacherClassSubjectExists == null)
            {
                result.AddError("Invaild TeacherClassSubjectId.");
                return result;
            }

            var checkPeriodExists = await _periodRepo.FirstOrDefaultAsync(model.PeriodId);
            if (checkPeriodExists == null)
            {
                result.AddError("Invaild PeriodId.");
                return result;
            }

            var checkTeacherHasOtherClassForPeriod = await _timeTableRepo.GetAll()
                .Where(m => m.TeacherClassSubject.TeacherId == checkTeacherClassSubjectExists.TeacherId && 
                m.PeriodId == checkPeriodExists.Id &&
                m.Day == model.Day)
                .FirstOrDefaultAsync();
            if (checkTeacherHasOtherClassForPeriod != null)
            {
                result.AddError("Teacher has another class for this period.");
                return result;
            }

            var t = new TimeTableCell
            {
                Day = model.Day,
                HasVirtual = model.HasVirtual,
                NoOfPeriod = 1,
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
                    ClassName = $"{x.TeacherClassSubject.SchoolClassSubject.SchoolClass.Name} {x.TeacherClassSubject.SchoolClassSubject.SchoolClass.ClassArm}",
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
