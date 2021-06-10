using AssessmentSvc.Core.Test.Setup;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using AssessmentSvc.Core.Interfaces;

namespace AssessmentSvc.Core.Test.Tests.ApprovedResultService
{
    public class ApprovedResultService_SubmitStudentResult
    {
        [Test]
        public async Task SubmitStudentResult_NewResult()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var approvedService = _setup.ServiceProvider.GetService<IApprovedResultService>();
                var context = _setup.DbContext;



                approvedService.SubmitStudentResult(newSchool);

                var schoolCheck = context.Schools.SingleOrDefault(x => x.Id == newSchool.Id);

                Assert.IsFalse(schoolCheck is null);
                Assert.AreEqual(schoolCheck.Name, newSchool.Name);
            }
        }

    }
}
