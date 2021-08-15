using AssessmentSvc.Core.Enumeration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssessmentSvc.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    [AllowAnonymous]
    public class EnumsController : BaseController
    {
        [HttpGet]
        public IActionResult ApprovalStatus()
        {
            var SO = Enum.GetValues(typeof(ApprovalStatus)).Cast<ApprovalStatus>().Select(m => new {
                val = m.ToString("d"),
                name = m.ToString().Replace('_', ' ')
            });
            return Ok(SO);
        }

        [HttpGet]
        public IActionResult PromotionMethod()
        {
            var SO = Enum.GetValues(typeof(PromotionMethod)).Cast<PromotionMethod>().Select(m => new
            {
                val = m.ToString("d"),
                name = m.ToString().Replace('_', ' ')
            }) ;
            return Ok(SO);
        }

        [HttpGet]
        public IActionResult PromotionStatus()
        {
            var SO = Enum.GetValues(typeof(PromotionStatus)).Cast<PromotionStatus>().Select(m => new {
                val = m.ToString("d"),
                name = m.ToString().Replace('_', ' ')
            });
            return Ok(SO);
        }

        [HttpGet]
        public IActionResult PromotionType()
        {
            var SO = Enum.GetValues(typeof(PromotionType)).Cast<PromotionType>().Select(m => new {
                val = m.ToString("d"),
                name = m.ToString().Replace('_', ' ')
            });
            return Ok(SO);
        }
    }
}
