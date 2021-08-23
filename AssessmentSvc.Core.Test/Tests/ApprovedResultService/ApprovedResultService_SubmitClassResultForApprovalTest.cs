using AssessmentSvc.Core.Test.Setup;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Models;
using FizzWare.NBuilder;
using AssessmentSvc.Core.ViewModels.Result;
using System.Linq;


namespace AssessmentSvc.Core.Test.Tests.ApprovedResultService
{
    [TestFixture]
    class ApprovedResultService_SubmitClassResultForApprovalTest
    {
        readonly string errorMessage1 = "Current term date has expired or its not setup";
        readonly string errorMessage2 = "No saved result records for student found.";

        [Test]
        public async Task SubmitClassResultForApprovalTest_sessionResultHasError()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                //Arrange
                var approvedService = _setup.ServiceProvider.GetService<IApprovedResultService>();
                var context = _setup.DbContext;

                var model = Builder<UpdateApprovedClassResultViewModel>.CreateNew().Build();

                //Act

                var act = await approvedService.SubmitClassResultForApproval(model);

                //Assert
                Assert.IsTrue(act.HasError);
                Assert.AreEqual(act.ErrorMessages[0], act.ValidationErrors[0].ErrorMessage);


            };
        }

        [Test]
        public async Task SubmitClassResultForApprovalTest()
        {

            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                //Arrange
                var approvedService = _setup.ServiceProvider.GetService<IApprovedResultService>();
                var context = _setup.DbContext;

                var session = Builder<SessionSetup>.CreateNew()
                    .With(p => p.IsCurrent = true)
                    .With(
                        p => p.Terms = (List<Term>)Builder<Term>.CreateListOfSize(1).All()
                            .With(p => p.StartDate = DateTime.Now.AddMonths(-1))
                            .With(p => p.EndDate = DateTime.Now.AddMonths(1))
                            .Build()
                        )
                    .Build();
                context.SessionSetups.Add(session);

                var student = Builder<Student>.CreateNew().With(
                        p => p.Class = Builder<SchoolClass>.CreateNew().Build()
                    )
                    .Build();
                context.Students.Add(student);

                var subject = Builder<Subject>.CreateNew().Build();
                context.Subjects.Add(subject);

                await context.SaveChangesAsync();

                var result = Builder<Result>.CreateNew()
                    .With(p => p.SessionSetupId = session.Id)
                    .With(p => p.ApprovedResultId = null)
                    .With(p => p.TermSequenceNumber = session.Terms[0].SequenceNumber)
                    .With(p => p.StudentId = student.Id)
                    .With(p => p.SubjectId = subject.Id)
                    .With(p => p.SchoolClassId = student.Class.Id)
                    .With(p => p.Scores = (List<Score>)Builder<Score>.CreateListOfSize(2).All().Build())
                    .Build();

                context.Results.Add(result);

                await context.SaveChangesAsync();

                var model = Builder<UpdateApprovedClassResultViewModel>.CreateNew().Build();
                //Act
                var rtn = await approvedService.SubmitClassResultForApproval(model);
                //Assert
                Assert.IsFalse(rtn.HasError);
                Assert.AreEqual("Record updated", rtn.Data);
            }
        }

        [Test]
        public async Task SubmitClassResultForApprovalTest_ResultNotFound()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                //Arrange
                var approvedService = _setup.ServiceProvider.GetService<IApprovedResultService>();
                var context = _setup.DbContext;

                var session = Builder<SessionSetup>.CreateNew()
                    .With(p => p.IsCurrent = true)
                    .With(
                        p => p.Terms = (List<Term>)Builder<Term>.CreateListOfSize(1).All()
                            .With(p => p.StartDate = DateTime.Now.AddMonths(-1))
                            .With(p => p.EndDate = DateTime.Now.AddMonths(1))
                            .Build()
                        )
                    .Build();
                context.SessionSetups.Add(session);

                var student = Builder<Student>.CreateNew().With(
                       p => p.Class = Builder<SchoolClass>.CreateNew().Build()
                   )
                   .Build();
                context.Students.Add(student);

                var subject = Builder<Subject>.CreateNew().Build();
                context.Subjects.Add(subject);

                await context.SaveChangesAsync();

                var model = Builder<UpdateApprovedClassResultViewModel>.CreateNew().Build();

                var rtn = await approvedService.SubmitClassResultForApproval(model);

                Assert.That(rtn.ErrorMessages.Contains(errorMessage2));
            }
        }

        [Test]
        public async Task SubmitClassResultForApprovalTest_currTermSequenceNull()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                //Arrange
                var approvedService = _setup.ServiceProvider.GetService<IApprovedResultService>();
                var context = _setup.DbContext;

                var session = Builder<SessionSetup>.CreateNew()
                    .With(p => p.IsCurrent = true)
                    .Build();
                context.SessionSetups.Add(session);

                await context.SaveChangesAsync();

                var model = Builder<UpdateApprovedClassResultViewModel>.CreateNew().Build();
                var rtn = await approvedService.SubmitClassResultForApproval(model);

                Assert.That(rtn.ErrorMessages.Contains(errorMessage1));
            }
        }

    }
}
