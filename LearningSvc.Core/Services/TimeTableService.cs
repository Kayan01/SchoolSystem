using LearningSvc.Core.Enumerations;
using LearningSvc.Core.Models.TimeTable;
using LearningSvc.Core.Services.Interfaces;
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
                Data = await _timeTableRepo.GetAll().Where(m=>m.TeacherId == teacherId && m.Day == day)
                    .Include(m=>m.Period).Include(m => m.Teacher).Include(m => m.Subject).Include(m => m.SchoolClass).ThenInclude(n=>n.Students)
                    .Select(x => (ClassSessionOutputVM)x).ToListAsync()
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

        public async Task<ResultModel<List<ClassSessionOutputVM>>> GetClassesForTeacherToday(long teacherId, WeekDays day, int Count)
        {
            var result = new ResultModel<List<ClassSessionOutputVM>>
            {
                Data = await _timeTableRepo.GetAll().Where(m => m.TeacherId == teacherId && m.Day == day)
                    .Include(m => m.Period).Include(m => m.Teacher).Include(m => m.Subject).Include(m => m.SchoolClass).ThenInclude(n => n.Students)
                    .Select(x => (ClassSessionOutputVM)x).Take(Count)
                    .ToListAsync()
            };
            return result;
        }

        public async Task<ResultModel<List<TimeTableCellVM>>> GetTimeTableCellsForClass(long classId)
        {
            var result = new ResultModel<List<TimeTableCellVM>>
            {
                Data = await _timeTableRepo.GetAll().Where(m => m.SchoolClassId == classId)
                    .Include(m => m.Period).Include(m => m.Teacher).Include(m => m.Subject).Include(m => m.SchoolClass)
                    .Select(x => (TimeTableCellVM)x).ToListAsync()
            };
            return result;
        }

        public async Task<ResultModel<List<TimeTableCellVM>>> GetTimeTableCellsForTeacher(long teacherId)
        {
            var result = new ResultModel<List<TimeTableCellVM>>
            {
                Data = await _timeTableRepo.GetAll().Where(m => m.TeacherId == teacherId)
                    .Include(m => m.Period).Include(m => m.Teacher).Include(m => m.Subject).Include(m => m.SchoolClass)
                    .Select(x => (TimeTableCellVM)x).ToListAsync()
            };
            return result;
        }

        public async Task<ResultModel<List<PeriodVM>>> SetupSchoolPeriods(List<PeriodVM> model)
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

            foreach (var item in model)
            {
                var p = new Period
                {
                    Name = item.Name,
                    Step = item.Step,
                    TimeFrom = item.TimeFrom,
                    TimeTo = item.TimeTo,
                    
                };
                item.Id = await _periodRepo.InsertAndGetIdAsync(p);

            }

            await _unitOfWork.SaveChangesAsync();
            
            result.Data = model;
            return result;
        }

        public async Task<ResultModel<List<TimeTableCellVM>>> SetupTimeTableCellsByClass(List<TimeTableCellVM> model, long classId)
        {
            var result = new ResultModel<List<TimeTableCellVM>>();

            //delete all timetable for a class to add new one
            await _timeTableRepo.DeleteAsync(m => m.SchoolClassId == classId);
            
            foreach (var item in model)
            {
                var t = new TimeTableCell
                {
                    Day = item.Day,
                    HasVirtual = item.HasVirtual,
                    NoOfPeriod = item.NoOfPeriod,
                    PeriodId = item.PeriodId,
                    SchoolClassId = item.SchoolClassId,
                    SubjectId = item.SubjectId,
                    TeacherId = item.TeacherId,
                    ZoomId = item.ZoomId,

                };
                item.Id = await _timeTableRepo.InsertAndGetIdAsync(t);

            }

            await _unitOfWork.SaveChangesAsync();

            result.Data = model;
            return result;
        }
    }
}
