using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Models;
using AssessmentSvc.Core.Test.Setup;
using FizzWare.NBuilder;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AssessmentSvc.Core.Test.Tests.ApprovedResultService
{
    class ApprovedResultService_GetStudentsWithApprovedResult
    {
        [Test]
        public async Task GetStudentsWithApprovedResult_AllResultExists()
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

                var rtn = await approvedService.GetStudentsWithApprovedResult(Class.Id, session.Id, session.Terms[0].SequenceNumber);

                Assert.IsFalse(rtn.HasError);
                Assert.IsNotNull(rtn.Data);
                Assert.AreEqual(3, rtn.Data.Count);
            }
        }

        [Test]
        public async Task GetStudentsWithApprovedResult_SomeStudentsResultDontExists()
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

                await context.SaveChangesAsync();

                var oldapprovedResults = new List<ApprovedResult>(
                    Builder<ApprovedResult>.CreateListOfSize(2)
                        .All().With(p => p.SessionId = session.Id)
                        .All().With(p => p.TermSequence = session.Terms[0].SequenceNumber)
                        .All().With(p => p.ClassTeacherComment = "old comment")
                        .All().With(p => p.TenantId = 1)
                        .All().With(p => p.IsDeleted = false)
                        .All().With(p => p.HeadTeacherApprovedStatus = Enumeration.ApprovalStatus.Approved)
                        .All().With(p => p.ClassTeacherApprovalStatus = Enumeration.ApprovalStatus.Approved)
                        .Build()
                );

                for (int i = 0; i < 2; i++)
                {
                    oldapprovedResults[i].StudentId = students[i].Id;
                }

                context.ApprovedResults.AddRange(oldapprovedResults);

                await context.SaveChangesAsync();

                var results = new List<Result>(
                    Builder<Result>.CreateListOfSize(2)
                        .All().With(p => p.SessionSetupId = session.Id)
                        .All().With(p => p.TermSequenceNumber = session.Terms[0].SequenceNumber)
                        .All().With(p => p.SubjectId = subject.Id)
                        .All().With(p => p.SchoolClassId = Class.Id)
                        .All().With(p => p.TenantId = 1)
                        .All().With(p => p.Scores = (List<Score>)Builder<Score>.CreateListOfSize(2).All().Build())
                        .Build()
                        );

                for (int i = 0; i < 2; i++)
                {
                    results[i].ApprovedResultId = oldapprovedResults[i].Id;
                    results[i].StudentId = students[i].Id;
                }
                context.Results.AddRange(results);

                await context.SaveChangesAsync();

                var rtn = await approvedService.GetStudentsWithApprovedResult(Class.Id, session.Id, session.Terms[0].SequenceNumber);

                Assert.IsFalse(rtn.HasError);
                Assert.IsNotNull(rtn.Data);
                Assert.AreEqual(2, rtn.Data.Count);
            }
        }

    }
}
