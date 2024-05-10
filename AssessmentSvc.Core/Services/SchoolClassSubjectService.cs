using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Models;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssessmentSvc.Core.Services
{
    public class SchoolClassSubjectService : ISchoolClassSubjectService
    {

        private readonly IRepository<SchoolClassSubject, long> _schoolClassSubjectRepo;
        private readonly IUnitOfWork _unitOfWork;

        public SchoolClassSubjectService(IUnitOfWork unitOfWork, IRepository<SchoolClassSubject, long> schoolClassSubjectRepo)
        {
            _unitOfWork = unitOfWork;
            _schoolClassSubjectRepo = schoolClassSubjectRepo;
        }

        public void AddOrUpdateClassSubjectFromBroadcast(List<SchoolClassSubjectSharedModel> model)
        {
            //list of broadcasted class ids
            var ids = model.Select(x => x.Id).ToList();

            //get all classes from db
            var schoolClassSubjects = _schoolClassSubjectRepo.GetAll().Where(x => ids.Contains(x.Id)).ToList();

            foreach (var cs in model)
            {
                var classSubject = schoolClassSubjects.FirstOrDefault(x => x.Id == cs.Id);
                if (classSubject == null)
                {
                    classSubject = _schoolClassSubjectRepo.Insert(new SchoolClassSubject
                    {
                        Id = cs.Id,
                    });
                }
                else
                {
                 
                    _schoolClassSubjectRepo.Update(classSubject);
                }

                classSubject.TenantId = cs.TenantId;
                classSubject.SchoolClassId = cs.SchoolClassId;
                classSubject.SubjectId = cs.SubjectId;

            }

            _unitOfWork.SaveChanges();
        }

    }
}
