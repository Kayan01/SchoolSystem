using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Test.Setup;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using FizzWare.NBuilder;
using AssessmentSvc.Core.Models;

namespace AssessmentSvc.Core.Test.Tests.PromotionSetupService
{
    public class PromotionSetupService_GetPromotionSetup
    {
        [Test]
        public async Task GetPromotionSetup_SetupExists()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var service = _setup.ServiceProvider.GetService<IPromotionSetupService>();
                var context = _setup.DbContext;

                var setup = Builder<PromotionSetup>.CreateNew()
                    .Build();
                var x = context.PromotionSetups.Add(setup);
                await context.SaveChangesAsync();

                var rtn = await service.GetPromotionSetup();

                Assert.IsFalse(rtn.HasError);
                Assert.IsNotNull(rtn.Data);
                Assert.AreEqual(setup.PromotionMethod, rtn.Data.PromotionMethod);
            }

        }

        [Test]
        public async Task GetApprovedResultForStudent_ApprovedResultDoesNotExist()
        {
            using (ServicesDISetup _setup = new ServicesDISetup())
            {
                var service = _setup.ServiceProvider.GetService<IPromotionSetupService>();
                var context = _setup.DbContext;

                var rtn = await service.GetPromotionSetup();

                Assert.IsFalse(rtn.HasError);
                Assert.IsNull(rtn.Data);
            }

        }

    }
}
