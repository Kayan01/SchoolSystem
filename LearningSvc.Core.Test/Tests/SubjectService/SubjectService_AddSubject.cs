using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Shared.ViewModels;
using System.Linq;
using LearningSvc.Core.Test.Setup;
using LearningSvc.Core.Interfaces;
using LearningSvc.Core.Models;
using LearningSvc.Core.ViewModels.Subject;

namespace LearningSvc.Core.Test.Tests.SubjectService
{
    [TestFixture]
    public class SubjectService_AddSubject
    {

        [Test]
        public async System.Threading.Tasks.Task AddSubject_SubjectDontExistAsync()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _subjectService = _setup.ServiceProvider.GetService<ISubjectService>();
                var context = _setup.DbContext;

                var newSubject = new SubjectInsertVM()
                {
                    IsActive = true,
                    Name = "subject",
                };

                var result = await _subjectService.AddSubject(newSubject);

                Assert.IsNotNull(result);
                Assert.IsFalse(result.HasError);
                Assert.AreEqual(newSubject.Name, result.Data.Name);

                var subjectCheck = context.Subjects.SingleOrDefault(x=>x.Id == result.Data.Id);

                Assert.IsNotNull(subjectCheck);
                Assert.AreEqual(newSubject.Name, subjectCheck.Name);
            }
        }

        [Test]
        public async System.Threading.Tasks.Task AddSubject_SubjectExistsAsync()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _subjectService = (ISubjectService)_setup.ServiceProvider.GetService(typeof(ISubjectService));
                var context = _setup.DbContext;


                var oldSubject = new Subject()
                {
                    IsActive = true,
                    Name = "subject",
                };
                context.Add(oldSubject);
                context.SaveChanges();

                var newSubject = new SubjectInsertVM()
                {
                    IsActive = true,
                    Name = "subject",
                };

                var result = await _subjectService.AddSubject(newSubject);

                Assert.IsNotNull(result);
                Assert.IsTrue(result.HasError);

            }
        }
    }
}
