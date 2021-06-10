using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Test.Setup;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Shared.ViewModels;
using System.Linq;
using AssessmentSvc.Core.Models;

namespace AssessmentSvc.Core.Test.Tests.SubjectService
{
    [TestFixture]
    public class SubjectService_AddOrUpdateSubjectFromBroadcast
    {

        [Test]
        public void AddOrUpdateSubjectFromBroadcast_AddSubject()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _subjectService = _setup.ServiceProvider.GetService<ISubjectService>();
                var context = _setup.DbContext;

                var newSubject = new SubjectSharedModel()
                {
                    IsActive = true,
                    Name = "subject",
                    Id = 1,
                };

                _subjectService.AddOrUpdateSubjectFromBroadcast(newSubject);

                var subjectCheck = context.Subjects.SingleOrDefault(x => x.Id == newSubject.Id);

                Assert.IsNotNull(subjectCheck);
                Assert.AreEqual(newSubject.Name, subjectCheck.Name);
            }
        }

        [Test]
        public void AddOrUpdateSubjectFromBroadcast_UpdateSubject()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _subjectService = (ISubjectService)_setup.ServiceProvider.GetService(typeof(ISubjectService));
                var context = _setup.DbContext;


                var oldSubject = new Subject()
                {
                    IsActive = true,
                    Name = "oldsubject",
                    Id = 1,
                };
                context.Add(oldSubject);
                context.SaveChanges();

                var newSubject = new SubjectSharedModel()
                {
                    IsActive = true,
                    Name = "newsubject",
                    Id = 1,
                };

                _subjectService.AddOrUpdateSubjectFromBroadcast(newSubject);

                var subjectCheck = context.Subjects.SingleOrDefault(x => x.Id == newSubject.Id);

                Assert.IsNotNull(subjectCheck);
                Assert.AreEqual(newSubject.Name, subjectCheck.Name);
            }
        }
    }
}
