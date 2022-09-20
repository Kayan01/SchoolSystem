using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels;
using Auth.Core.ViewModels.School;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;

namespace UserManagement.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class SchoolController : BaseController
    {
        private readonly ISchoolService _schoolService;
        private readonly IRecurringJobManager _recurringJobManager;

        public SchoolController(ISchoolService schoolService, IRecurringJobManager recurringJobManager)
        {
            _schoolService = schoolService;
            _recurringJobManager = recurringJobManager;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<SchoolVM>), 200)]
        public async Task<IActionResult> AddSchool([FromForm]CreateSchoolVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<object>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _schoolService.AddSchool(model);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        public async Task<IActionResult> BulkAddSchool([FromForm]IFormFile file)
        {
            if (file == null)
                return ApiResponse<string>(errors: "No file uploaded");

            if (!ModelState.IsValid)
                return ApiResponse<object>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _schoolService.AddBulkSchool(file);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<SchoolVM>>), 200)]
        public async Task<IActionResult> GetSchools([FromQuery] QueryModel vM)
        {
            try
            {
                var result = await _schoolService.GetAllSchools(vM);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items, totalCount: result.Data.TotalItemCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> GetSchoolExcelUploadData()
        {
            try
            {
                var result = await _schoolService.GetSchoolExcelSheet();
                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: Convert.ToBase64String(result.Data), totalCount: 1);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{id}")]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<SchoolDetailVM>), 200)]
        public async Task<IActionResult> GetSchool(long id)
        {
            if (id <= 0)
            {
                return ApiResponse<string>(errors: "Please provide valid school Id");
            }

            try
            {
                var result = await _schoolService.GetSchoolById(id);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<SchoolNameAndLogoVM>), 200)]
        public async Task<IActionResult> GetSchoolNameAndLogo(long id)
        {
            if (id <= 0)
            {
                return ApiResponse<SchoolNameAndLogoVM>(errors: "Please provide valid school Id");
            }
            try
            {
                var result = await _schoolService.GetSchoolNameAndLogoById(id);
                if (result.HasError)
                    return ApiResponse<SchoolNameAndLogoVM>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{domain}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<SchoolNameAndLogoVM>), 200)]
        public async Task<IActionResult> GetSchoolNameAndLogoByDomain(string domain)
        {
            if (string.IsNullOrWhiteSpace(domain))
            {
                return ApiResponse<SchoolNameAndLogoVM>(errors: "Please provide valid school Domain");
            }
            try
            {
                var result = await _schoolService.GetSchoolNameAndLogoByDomain(domain);
                if (result.HasError)
                    return ApiResponse<SchoolNameAndLogoVM>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("{id}")]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<SchoolVM>), 200)]
        public async Task<IActionResult> UpdateSchool([FromForm]UpdateSchoolVM vM,[FromRoute] long id)
        {
            if (!ModelState.IsValid)
                return ApiResponse<object>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            if (id < 1)
            {
                return ApiResponse<string>(errors: "Invalid school Id");
            }

            try
            {
                var result = await _schoolService.UpdateSchool(vM, id);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpDelete("{id}")]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        public async Task<IActionResult> DeleteSchool(long id)
        {
            if (id < 1)
                return ApiResponse<string>(errors: "Invalid school Id");

            try
            {
                var result = await _schoolService.DeleteSchool(id);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{id}")]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        public async Task<IActionResult> EnableSchool(long id)
        {
            if (id < 1)
                return ApiResponse<string>(errors: "Invalid school Id");

            try
            {
                var result = await _schoolService.ActivateSchool(id);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
        [HttpGet("{id}")]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        public async Task<IActionResult> DisableSchool(long id)
        {
            if (id < 1)
                return ApiResponse<string>(errors: "Invalid school Id");

            try
            {
                var result = await _schoolService.DeActivateSchool(id);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> TriggerCheckForSchoolWithExpiredSubcriptionReccuringJob()
        {
            try
            {
                Console.WriteLine("Triggering Endpoint Uisng HangFire");
                _recurringJobManager.AddOrUpdate("jobId", () => _schoolService.CheckForSchoolWithExpiredSubcription(), Cron.Daily);
                Console.WriteLine("Hangfire Triggering Done");
                return ApiResponse<object>(message: "Enpoint Successfully Triggered", codes: ApiResponseCodes.OK);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{schoolId}")]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> NotifySubcriptionExpirationDateToAdmin(long schoolId)
        {
            try
            {

                var result = await _schoolService.NotifySubcriptionExpirationDateToAdmin(schoolId);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());

                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data : result.Message);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        //[Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<SchoolVM>>), 200)]
        public async Task<IActionResult> GetTotalUsersOnPlatform()
        {
            try
            {
                var result = await _schoolService.TotalUsersOnPlatform();
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Total user count retrieved Successfully.",codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<ExportPayloadVM>), 200)]
        public async Task<IActionResult> ExportSchoolScrib()
        {
            var payload = new ExportPayloadVM();
            try
            {
                var res = await _schoolService.ExportSchoolSubscriptionDetails();
                if (res.Data != null)
                {
                    payload = new ExportPayloadVM
                    {
                        FileName = "SubscriptionDetails",
                        Base64String = Convert.ToBase64String(res.Data),
                    };
                }

                return ApiResponse(data: payload, message: "Export Successful",totalCount: res.TotalCount);
            }
            catch (Exception ex)
            {

                return HandleError(ex);
            }

        }

    }
}