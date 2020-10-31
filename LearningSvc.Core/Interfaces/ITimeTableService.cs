using LearningSvc.Core.Enumerations;
using LearningSvc.Core.ViewModels.TimeTable;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningSvc.Core.Interfaces
{
    public interface ITimeTableService
    {
        Task<ResultModel<List<PeriodVM>>> SetupSchoolPeriods(List<PeriodVM> model);
        Task<ResultModel<List<PeriodVM>>> GetAllPeriodForSchool();
        Task<ResultModel<List<TimeTableCellVM>>> SetupTimeTableCellsByClass(List<TimeTableCellVM> model, long classId);
        Task<ResultModel<List<TimeTableCellVM>>> GetTimeTableCellsForTeacher(long teacherId);
        Task<ResultModel<List<TimeTableCellVM>>> GetTimeTableCellsForClass(long classId);
        Task<ResultModel<List<ClassSessionOutputVM>>> GetAllClassesForTeacherToday(long teacherId, WeekDays day);
        Task<ResultModel<List<ClassSessionOutputVM>>> GetNextClassesForTeacherToday(long teacherId, WeekDays day, int curPeriod, int Count);
    }
}
