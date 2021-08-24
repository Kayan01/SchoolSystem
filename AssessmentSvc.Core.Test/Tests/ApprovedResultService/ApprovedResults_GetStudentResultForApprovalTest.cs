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
    class ApprovedResults_GetStudentResultForApprovalTest
    {
        [Test]
        public async Task ApproveResult_GetStudentsResultForApproval_NoTerm()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var approvedService = _setup.ServiceProvider.GetService<IApprovedResultService>();
                var context = _setup.DbContext;

                string expectedOutput = "Current term date has expired or its not setup";

                var session = Builder<SessionSetup>.CreateNew()
                    .With(p => p.IsCurrent = true)
                    .With(
                        p => p.Terms = (List<Term>)Builder<Term>.CreateListOfSize(1).All()
                            .With(p => p.StartDate = DateTime.Now.AddMonths(1))
                            .With(p => p.EndDate = DateTime.Now.AddMonths(-1))
                            .Build()
                        )
                    .Build();
                context.SessionSetups.Add(session);

                await context.SaveChangesAsync();

                var model = Builder<GetStudentResultForApproval>.CreateNew().Build();

                var result = await approvedService.GetStudentResultForApproval(model);

                Assert.That(result.ErrorMessages.Contains(expectedOutput));
            }
        }

        [Test]
        public async Task ApproveResult_GetStudentsResultForApproval_NoResult()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var approvedService = _setup.ServiceProvider.GetService<IApprovedResultService>();
                var context = _setup.DbContext;

                var expectedOutPut = "No result for approval found";

                var session = Builder<SessionSetup>.CreateNew()
                    .With(p => p.IsCurrent = true)
                    .With(
                        p => p.Terms = (List<Term>)Builder<Term>.CreateListOfSize(1).All()
                            .With(p => p.StartDate = DateTime.Now.AddMonths(1))
                            .With(p => p.EndDate = DateTime.Now.AddMonths(-1))
                            .Build()
                        )
                    .Build();
                context.SessionSetups.Add(session);

                await context.SaveChangesAsync();

                var model = Builder<GetStudentResultForApproval>.CreateNew().Build();

                var result = await approvedService.GetStudentResultForApproval(model);

                Assert.That(result.ErrorMessages.Contains(expectedOutPut));
            }
        }

        [Test]
        public async Task ApproveResult_GetStudentsResultForApproval()
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

                var oldapprovedResult = Builder<ApprovedResult>.CreateNew().Build();
                context.ApprovedResults.Add(oldapprovedResult);

                var result = Builder<Result>.CreateNew()
                     .With(p => p.SessionSetupId = session.Id)
                     .With(p => p.ApprovedResultId = oldapprovedResult.Id)
                     .With(p => p.TermSequenceNumber = session.Terms[0].SequenceNumber)
                     .With(p => p.StudentId = student.Id)
                     .With(p => p.SubjectId = subject.Id)
                     .With(p => p.SchoolClassId = student.Class.Id)
                     .With(p => p.Scores = (List<Score>)Builder<Score>.CreateListOfSize(2).All().Build())
                     .Build();

                context.Results.Add(result);

                var gradeSetup = Builder<GradeSetup>.CreateNew().Build();
                context.GradeSetups.Add(gradeSetup);

                var expected = Builder<GetApprovedStudentResultViewModel>.CreateNew().Build();

                await context.SaveChangesAsync();


                var model = Builder<GetStudentResultForApproval>.CreateNew().Build();

                //act
                var act = await approvedService.GetStudentResultForApproval(model);

                //Assert
                Assert.IsFalse(act.HasError);
                Assert.AreEqual(expected.HeadTeacherApprovalStatus, act.Data.HeadTeacherApprovalStatus);
            }
        }
    }
}
