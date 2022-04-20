using LearningSvc.Core.Interfaces;
using LearningSvc.Core.Test.Setup;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using LearningSvc.Core.Models;
using Shared.ViewModels;
using System.Linq;

namespace LearningSvc.Core.Test.Tests.SubjectService
{
    [TestFixture]
    public class SubjectService_GetAllSubjects
    {
        [Test]
        public async System.Threading.Tasks.Task GetAllSubjects_()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _subjectService = _setup.ServiceProvider.GetService<ISubjectService>();
                var context = _setup.DbContext;

                var subjects = new List<Subject>()
                {
                    new Subject()
                    {
                        IsActive = true,
                        Name = "subject1",
                    },
                    new Subject()
                    {
                        IsActive = true,
                        Name = "subject2",
                    },
                    new Subject()
                    {
                        IsActive = true,
                        Name = "subject3",
                    },
                };
                context.Subjects.AddRange(subjects);
                context.SaveChanges();

                var result = await _subjectService.GetAllSubjects();

                Assert.IsNotNull(result);
                Assert.IsFalse(result.HasError);
                Assert.AreEqual(3, result.Data.Count);

            }
        }

        [Test]
        public async System.Threading.Tasks.Task GetAllSubjects_Paginated()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var _subjectService = _setup.ServiceProvider.GetService<ISubjectService>();
                var context = _setup.DbContext;

                var subjects = new List<Subject>()
                {
                    new Subject()
                    {
                        IsActive = true,
                        Name = "subject1",
                    },
                    new Subject()
                    {
                        IsActive = true,
                        Name = "subject2",
                    },
                    new Subject()
                    {
                        IsActive = true,
                        Name = "subject3",
                    },
                    new Subject()
                    {
                        IsActive = true,
                        Name = "subject4",
                    },
                    new Subject()
                    {
                        IsActive = true,
                        Name = "subject5",
                    },
                    new Subject()
                    {
                        IsActive = true,
                        Name = "subject6",
                    },
                };
                context.Subjects.AddRange(subjects);
                context.SaveChanges();


                var result = await _subjectService.GetAllSubjects(new QueryModel() { PageIndex = 1, PageSize=2 }) ;

                Assert.IsNotNull(result);
                Assert.IsFalse(result.HasError);
                var items = result.Data.Items.ToList();
                Assert.AreEqual(2, items.Count);
                Assert.AreEqual(subjects[0].Name, items[0].Name);

                result = await _subjectService.GetAllSubjects(new QueryModel() { PageIndex = 2, PageSize = 2 });

                Assert.IsNotNull(result);
                Assert.IsFalse(result.HasError);
                items = result.Data.Items.ToList();
                Assert.AreEqual(2, items.Count);
                Assert.AreEqual(subjects[2].Name, items[0].Name);

            }
        }


    }
}
