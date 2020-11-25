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
        Task<ResultModel<List<PeriodVM>>> SetupSchoolPeriods(List<PeriodInsertVM> model);
        Task<ResultModel<List<PeriodVM>>> GetAllPeriodForSchool();
        Task<ResultModel<TimeTableCellVM>> AddTimeTableCell(TimeTableCellInsertVM model);
        Task<ResultModel<string>> DeleteTimeTableCell(long TimeTableCellId);
        Task<ResultModel<List<TimeTableCellVM>>> GetTimeTableCellsForTeacher(long currentUserId);
        Task<ResultModel<List<TimeTableCellVM>>> GetTimeTableCellsForClass(long classId);
        Task<ResultModel<List<ClassSessionOutputVM>>> GetAllClassesForTeacherToday(long currentUserId, WeekDays day);
        Task<ResultModel<List<ClassSessionOutputVM>>> GetNextClassesForTeacherToday(long currentUserId, WeekDays day, int curPeriod, int Count);
    }
}
