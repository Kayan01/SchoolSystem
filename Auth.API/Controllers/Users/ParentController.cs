using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Core.Interfaces.Users;
using Auth.Core.ViewModels.Parent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;

namespace Auth.API.Controllers.Users
{
    [Route("api/v1/[controller]/[action]")]
    [AllowAnonymous]
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
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items);
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
                return ApiResponse<object>(errors: ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);
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


        [HttpPost("{Id}")]
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

        //[HttpPost]
        //[ProducesResponseType(typeof(ApiResponse<object>), 200)]
        //public async Task<IActionResult> AddStaff(StaffVM model)
        //{

        //}

    }
}
