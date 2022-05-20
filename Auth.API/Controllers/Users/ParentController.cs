using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Core.Interfaces.Users;
using Auth.Core.ViewModels.Parent;
using Auth.Core.ViewModels.School;
using Auth.Core.ViewModels.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;

namespace Auth.API.Controllers.Users
{
    [Route("api/v1/[controller]/[action]")]
    [Authorize]
    public class ParentController : BaseController
    {
        private readonly IParentService _parentService;
        public ParentController(IParentService parentService)
        {
            _parentService = parentService;
        }


        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ParentListVM>>), 200)]
        public async Task<IActionResult> GetAllParents(QueryModel model)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);
            }


            try
            {
                var result = await _parentService.GetAllParents(model);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items, totalCount: result.Data.TotalItemCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }


        }

        [HttpGet("{schoolid}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ParentListVM>>), 200)]
        public async Task<IActionResult> GetAllParentsInSchool(long schoolid, QueryModel model)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);
            }


            try
            {
                var result = await _parentService.GetAllParentsInSchool(schoolid,model);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items, totalCount : result.Data.TotalItemCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }

        }

        [HttpGet("{studId}")]
        [ProducesResponseType(typeof(ApiResponse<ParentDetailVM>), 200)]
        public async Task<IActionResult> GetParentsForStudent(long studId)
        {
            if (studId < 1)
            {
                return ApiResponse<string>(errors: "Invalid Id provided", codes: ApiResponseCodes.INVALID_REQUEST);
            }

            try
            {
                var result = await _parentService.GetParentsForStudent(studId);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }



        }


        [HttpGet()]
        [ProducesResponseType(typeof(ApiResponse<List<SchoolParentViewModel>>), 200)]
        public async Task<IActionResult> GetStudentsSchools()
        {
            
            try
            {
                var result = await _parentService.GetStudentsSchools(CurrentUser.UserId);

                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());

                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }



        }


        [HttpGet()]
        [ProducesResponseType(typeof(ApiResponse<List<StudentParentVM>>), 200)]
        public async Task<IActionResult> GetStudentsInSchool()
        {

            try
            {
                var result = await _parentService.GetStudentsInSchool(CurrentUser.UserId);

                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());

                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }



        }

        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(ApiResponse<ParentDetailVM>), 200)]
        public async Task<IActionResult> GetParentById(long Id)
        {
            if (Id < 1)
            {
                return ApiResponse<string>(errors: "Invalid Id provided", codes: ApiResponseCodes.INVALID_REQUEST);
            }

            try
            {
                var result = await _parentService.GetParentById(Id);

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
        [ProducesResponseType(typeof(ApiResponse<ParentDetailVM>), 200)]
        public async Task<IActionResult> AddNewParent([FromForm] AddParentVM model)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse<object>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);
            }


            try
            {
                var result = await _parentService.AddNewParent(model);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }


        [HttpPut("{Id}")]
        [ProducesResponseType(typeof(ApiResponse<ResultModel<ParentDetailVM>>), 200)]
        public async Task<IActionResult> UpdateParent(long Id,[FromForm]UpdateParentVM vm)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse<object>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);
            }

            try
            {
                var result = await _parentService.UpdateParent(Id, vm);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpDelete]
        [ProducesResponseType(typeof(ApiResponse<ResultModel<string>>), 200)]
        public async Task<IActionResult> DeleteParent(long Id)
        {
            if (Id < 1)
            {
                return ApiResponse<string>(errors: "Invalid Id provided", codes: ApiResponseCodes.INVALID_REQUEST);
            }

            try
            {
                var result = await _parentService.DeleteParent(Id);

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
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> GetParentsExcelSheet()
        {
            try
            {
                var result = await _parentService.GetParentExcelSheet();
                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: Convert.ToBase64String(result.Data), totalCount: 1);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        public async Task<IActionResult> AddBulkParent([FromForm] IFormFile file)
        {
            if (file == null)
                return ApiResponse<string>(errors: "No file uploaded");

            if (!ModelState.IsValid)
                return ApiResponse<object>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _parentService.UploadBulkParentData(file);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }


        [HttpGet("{FirstName}")]
        [ProducesResponseType(typeof(ApiResponse<ParentDetailVM>), 200)]
        public async Task<IActionResult> GetParentByFirstName(QueryModel vm, string FirstName)
        {
            if (FirstName == null)
            {
                return ApiResponse<string>(errors: "Invalid First Name provided", codes: ApiResponseCodes.INVALID_REQUEST);
            }

            try
            {
                var result = await _parentService.GetParentByName(vm,FirstName);

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
        [ProducesResponseType(typeof(ApiResponse<ParentDetailVM>), 200)]
        public async Task<IActionResult> GetParentBySchoolIdAndName(QueryModel vm,[FromQuery]SearchParentVm model)
        {
            if (model == null)
            {
                return ApiResponse<string>(errors: "Invalid First Name provided", codes: ApiResponseCodes.INVALID_REQUEST);
            }

            try
            {
                var result = await _parentService.GetParentBySchoolAndName(vm,model);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items, totalCount: result.Data.TotalItemCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
