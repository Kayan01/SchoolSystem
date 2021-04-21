using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels.Enums;
using Microsoft.AspNetCore.Authorization;
using Shared.ViewModels;
using Shared.Utils;
using System.Collections.Generic;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;

namespace FinanceSvc.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class TestController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConverter _converter;

        public TestController(IWebHostEnvironment hostingEnvironment, IConverter converter) 
        {
            _converter = converter;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("{title}")]
        [AllowAnonymous]
        public async Task<IActionResult> TestBroadcast(string title)
        {
            try
            {
                return ApiResponse<string>(message: "Successful", codes: ApiResponseCodes.OK, data: "Sent");
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        //ToRemove
        [HttpGet()]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<byte[]>), 200)]
        public IActionResult TestSinglePagePdf()
        {
            try
            {
                var data = new { Header = "Test PDF Header", Body = "Test PDF Body" };
                var result = PdfHelper.BuildPdfFile(_converter, _hostingEnvironment, CoreConstants.TestPdfTemplatePath1, data, true);

                return File(result, "application/octet-stream", "TestSinglePage.pdf");
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        //ToRemove
        [HttpGet()]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<byte[]>), 200)]
        public IActionResult TestListPdf()
        {
            try
            {
                var tableConfig = new TableAttributeConfig
                {
                    TableAttributes = new { @style = "width: 100%; border-collapse: unset; border-spacing: 0;" },
                    ThAttributes = new { @style = "text-align: left;font-size: 11.2247px;line-height: 133.9%;color: #111118; font-weight: bold;border-bottom: 1px solid #111118 ;" },
                    TdAttributes = new { @style = "font-size: 11.2247px;line-height: 133.9%;color: #111118; font-weight: bold;  margin-top: 15px;" },
                };

                var mainData = new
                {
                    Id = "#007",
                    Fullname = "John Doe",
                    Address = "14B Karimu Ikotun Street, Victoria Island Lagos",
                    Date = DateTime.Now.ToLongDateString()
                };

                var listData = new List<dynamic> { new { SN = "1", Body = "Test PDF Body Data row 1" }, new { SN = "2", Body = "Test PDF Body Data row 2" }, new { SN = "2", Body = "Test PDF Body Data row 3" } };
                var result = PdfHelper.BuildPdfFile(_converter, _hostingEnvironment, CoreConstants.TestPdfTemplatePath2, mainData, listData, tableConfig, false);

                return File(result, "application/octet-stream", "TestListPage.pdf");
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet()]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<byte[]>), 200)]
        public async Task<IActionResult> TestSinglePagePdf2()
        {
            try
            {
                var data = new { Name = "Test PDF School", City = "Ikeja", State="Lagos" };
                var result = PdfHelper.BuildPdfFile(_converter, _hostingEnvironment, CoreConstants.InvoicePdfTemplatePath, data, true);

                return File(result, "application/octet-stream", "TestSinglePage.pdf");
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
