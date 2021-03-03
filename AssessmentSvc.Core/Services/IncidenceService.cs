using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Models;
using AssessmentSvc.Core.ViewModels.AssessmentSetup;
using AssessmentSvc.Core.ViewModels.Result;
using AssessmentSvc.Core.ViewModels.Student;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AssessmentSvc.Core.ViewModels;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;

namespace AssessmentSvc.Core.Services
{
    public class IncidenceService : IIncidenceService
    {
        private readonly IRepository<StudentIncidence, long> _incidenceRepository;
        private readonly IUnitOfWork _unitOfWork;
        

        public IncidenceService(IUnitOfWork unitOfWork,
            IRepository<StudentIncidence, long> incidenceRepository)
        {
            _unitOfWork = unitOfWork;
            _incidenceRepository = incidenceRepository;
        }


        public async Task<ResultModel<string>> InsertIncidenceReport(AddIncidenceVm model)
        {
            await _incidenceRepository.InsertAsync(new StudentIncidence
            {
                OccurrenceDate = model.OccurenceDate,
                Description = model.Description,

                SessionId = model.SessionId,
                SchoolClassId = model.ClassId,
                TermSequenceNumber = model.TermSequence,


            });

            await _unitOfWork.SaveChangesAsync();

            return new ResultModel<string>(data: "Saved");
        }

        public async Task<ResultModel<List<GetIncidenceVm>>> GetIncidenceReport(GetIncidenceQueryVm model)
        {
            var data =await _incidenceRepository.GetAll().Where(x =>
                x.SessionId == model.SessionId &&
                x.SchoolClassId == model.ClassId &&
                x.TermSequenceNumber == model.TermSequence &&
                x.StudentId == model.StudentId)
                .Select(r=> new GetIncidenceVm
                {
                     Description =  r.Description,
                      OccurenceDate = r.OccurrenceDate
                })
                .ToListAsync();


            return new ResultModel<List<GetIncidenceVm>>(data: data);
        }
    }
}
