using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;
using AssessmentSvc.Core.Test.Setup;
using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Models;
using FizzWare.NBuilder;
using AssessmentSvc.Core.ViewModels.Result;
using System.Linq;

namespace AssessmentSvc.Core.Test.Tests.ApprovedResultService
{
    class ApprovedResultService_MailResult
    {
        [Test]
        public async Task MailResult_AllResultExists()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
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

                var Class = Builder<SchoolClass>.CreateNew().Build();
                var students = Builder<Student>.CreateListOfSize(3)
                    .All().With(p => p.Class = Class)
                    .All().With(p => p.TenantId = 1)
                    .All().With(p => p.IsDeleted = false)
                    .Build();
                context.Students.AddRange(students);

                var grade = Builder<GradeSetup>.CreateListOfSize(5).All().With(
                        p => p.TenantId = 1
                    ).Build();
                context.GradeSetups.AddRange(grade);

                var subject = Builder<Subject>.CreateNew().Build();
                context.Subjects.Add(subject);

                foreach (var student in students)
                {
                    var behaviour = Builder<BehaviourResult>.CreateListOfSize(3)
                            .All().With(p => p.Id = 0)
                            .All().With(p => p.SessionId = session.Id)
                            .All().With(p => p.TermSequenceNumber = session.Terms[0].SequenceNumber)
                            .All().With(p => p.TenantId = 1)
                            .All().With(p => p.IsDeleted = false)
                            .All().With(p => p.StudentId = student.Id)
                            .All().With(p => p.SchoolClassId = Class.Id)
                            .Build();
                    context.BehaviourResults.AddRange(behaviour);
                }
                await context.SaveChangesAsync();

                var oldapprovedResults = new List<ApprovedResult>(
                    Builder<ApprovedResult>.CreateListOfSize(3)
                        .All().With(p => p.SessionId = session.Id)
                        .All().With(p => p.TermSequence = session.Terms[0].SequenceNumber)
                        .All().With(p => p.ClassTeacherComment = "old comment")
                        .All().With(p => p.TenantId = 1)
                        .All().With(p => p.IsDeleted = false)
                        .All().With(p => p.HeadTeacherApprovedStatus = Enumeration.ApprovalStatus.Approved)
                        .All().With(p => p.ClassTeacherApprovalStatus = Enumeration.ApprovalStatus.Approved)
                        .Build()
                );

                for (int i = 0; i < 3; i++)
                {
                    oldapprovedResults[i].StudentId = students[i].Id;
                }

                context.ApprovedResults.AddRange(oldapprovedResults);

                await context.SaveChangesAsync();

                var results = new List<Result>(
                    Builder<Result>.CreateListOfSize(3)
                        .All().With(p => p.SessionSetupId = session.Id)
                        .All().With(p => p.TermSequenceNumber = session.Terms[0].SequenceNumber)
                        .All().With(p => p.SubjectId = subject.Id)
                        .All().With(p => p.SchoolClassId = Class.Id)
                        .All().With(p => p.TenantId = 1)
                        .All().With(p => p.Scores = (List<Score>)Builder<Score>.CreateListOfSize(2).All().Build())
                        .Build()
                        );

                for (int i = 0; i < 3; i++)
                {
                    results[i].ApprovedResultId = oldapprovedResults[i].Id;
                    results[i].StudentId = students[i].Id;
                }
                context.Results.AddRange(results);

                await context.SaveChangesAsync();

                var rtn = await approvedService.MailResult(new MailResultVM()
                {
                    classId = Class.Id,
                    StudentIds = students.Select(m => m.Id).ToArray(),
                    curSessionId = session.Id,
                    termSequenceNumber = session.Terms[0].SequenceNumber,
                    ResultPageURL = ""
                }) ;

                Assert.IsFalse(rtn.HasError);
                Assert.IsNotNull(rtn.Data);
                Assert.AreEqual("Successful", rtn.Data);
            }
        }

    }
}
