using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Models;
using AssessmentSvc.Core.Test.Setup;
using FizzWare.NBuilder;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AssessmentSvc.Core.Test.Tests.ApprovedResultService
{
    class ApprovedResultService_GetApprovedResultForStudent
    {
        [Test]
        public async Task GetApprovedResultForStudent_ApprovedResultExists()
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

                var student = Builder<Student>.CreateNew().With(
                        p => p.Class = Builder<SchoolClass>.CreateNew().Build()
                    )
                    .Build();
                context.Students.Add(student);

                var grade = Builder<GradeSetup>.CreateListOfSize(5).Build();
                context.GradeSetups.AddRange(grade);

                var subject = Builder<Subject>.CreateNew().Build();
                context.Subjects.Add(subject);

                await context.SaveChangesAsync();

                var oldapprovedResult = Builder<ApprovedResult>.CreateNew()
                    .With(p => p.SessionId = session.Id)
                    .With(p => p.TermSequence = session.Terms[0].SequenceNumber)
                    .With(p => p.StudentId = student.Id)
                    .With(p => p.ClassTeacherComment = "old comment")
                    .Build();

                context.ApprovedResults.Add(oldapprovedResult);

                await context.SaveChangesAsync();

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

                await context.SaveChangesAsync();

                var rtn = await approvedService.GetApprovedResultForStudent(student.Class.Id, student.Id, null, session.Id, session.Terms[0].SequenceNumber);

                Assert.IsFalse(rtn.HasError);
                Assert.IsNotNull(rtn.Data);
                Assert.IsNotNull(oldapprovedResult.HeadTeacherComment, rtn.Data.HeadTeacherComment);
            }

        }

        [Test]
        public async Task GetApprovedResultForStudent_ApprovedResultDoesNotExist()
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

                var student = Builder<Student>.CreateNew().With(
                        p => p.Class = Builder<SchoolClass>.CreateNew().Build()
                    )
                    .Build();
                context.Students.Add(student);

                var grade = Builder<GradeSetup>.CreateListOfSize(5).Build();
                context.GradeSetups.AddRange(grade);

                var subject = Builder<Subject>.CreateNew().Build();
                context.Subjects.Add(subject);

                await context.SaveChangesAsync();

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

                var rtn = await approvedService.GetApprovedResultForStudent(student.Class.Id, student.Id, null, session.Id, session.Terms[0].SequenceNumber);

                Assert.IsTrue(rtn.HasError);
                Assert.AreEqual("No result found in current term and session", rtn.ErrorMessages[0]);
            }

        }

    }
}
