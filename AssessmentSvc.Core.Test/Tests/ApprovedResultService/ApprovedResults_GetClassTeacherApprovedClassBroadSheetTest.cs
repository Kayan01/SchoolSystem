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
    class ApprovedResults_GetClassTeacherApprovedClassBroadSheetTest
    {
        [Test]
        public async Task GetClassTeacherApprovedClassBroadSheetTest()
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

                var classes = Builder<SchoolClass>.CreateNew().Build();
                context.SchoolClasses.Add(classes);
                
                var Studentclass = Builder<Student>.CreateNew()
                    .Build();
                context.Students.Add(Studentclass);

                var subject = Builder<Subject>.CreateNew().Build();
                context.Subjects.Add(subject);

                var result = Builder<Result>.CreateNew()
                   .With(p => p.SessionSetupId = session.Id)
                   .With(p => p.ApprovedResultId)
                   .With(p => p.TermSequenceNumber = session.Terms[0].SequenceNumber)
                   .With(p => p.StudentId = Studentclass.Id)
                   .With(p => p.SubjectId = subject.Id)
                   .With(p => p.SchoolClassId = Studentclass.Class.Id)
                   .With(p => p.Scores = (List<Score>)Builder<Score>.CreateListOfSize(2).All().Build())
                   .Build();
                context.Results.Add(result);

                var approvedResult = Builder<ApprovedResult>.CreateNew()
                    .With(p => p.SessionId = session.Id)
                    .With(p => p.TermSequence = session.Terms[0].SequenceNumber)
                    .With(p => p.StudentId = Studentclass.Id)
                    .With(p => p.ClassTeacherComment = "new Comment")
                    .Build();

                context.ApprovedResults.Add(approvedResult);

               

                await context.SaveChangesAsync();

                var expectedResult = Builder<List<ResultBroadSheet>>.CreateNew().Build().ToString();
                

                //Act

                var Id = (long)(Studentclass.ClassId);
                var act = approvedService.GetClassTeacherApprovedClassBroadSheet(Id);

                //Assert
                Assert.IsTrue(act.IsCompleted);
                Assert.AreEqual(expectedResult,act.Result.Data.ToString());
            }
            
        }
    }
}
