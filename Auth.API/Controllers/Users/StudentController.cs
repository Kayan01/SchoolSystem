using System;
using System.Threading.Tasks;
using Auth.Core.Services.Interfaces;
using Auth.Core.ViewModels.Student;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.AspNetCore.Policy;
using Shared.Pagination;
using Shared.Permissions;
using Shared.ViewModels;
using Shared.ViewModels.Enums;


namespace Auth.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class StudentController : BaseController
    {

        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(ApiResponse<StudentDetailVM>), 200)]
        public async Task<IActionResult> GetStudentProfile(long Id)
        {

            if (Id < 1)
                return ApiResponse<string>(errors: "Invalid Id");
           
            try
            {
                var result = await _studentService.GetStudentProfileById(Id);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [RequiresPermission(Permission.STUDENT_CREATE)]
        [ProducesResponseType(typeof(ApiResponse<StudentVM>), 200)]
        public async Task<IActionResult> AddStudent([FromForm]CreateStudentVM model)
        {

            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<object>(ListModelErrors, codes : ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _studentService.AddStudentToSchool(model);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }



        [HttpGet]
        [RequiresPermission(Permission.STUDENT_READ)]
        [ProducesResponseType(typeof(ApiResponse<PaginatedModel<StudentVM>>), 200)]
        public async Task<IActionResult> GetAllStudent([FromQuery] QueryModel vM)
        { 
            try
            {
                var result = await _studentService.GetAllStudentsInSchool(vM);

                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items, totalCount: result.Data.TotalItemCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{id}")]
        [RequiresPermission(Permission.STUDENT_READ)]
        [ProducesResponseType(typeof(ApiResponse<StudentVM>), 200)]
        public async Task<IActionResult> GetStudentById(long id)
        {
            if(id < 1)
                return ApiResponse<string>(errors: "Please provide valid Student Id");

            try
            {
                var result = await _studentService.GetStudentById(id);
                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{classId}")]
        [RequiresPermission(Permission.STUDENT_READ)]
        [ProducesResponseType(typeof(ApiResponse<StudentVM>), 200)]
        public async Task<IActionResult> GetStudentInClass([FromQuery] QueryModel vM, long classId)
        {
            if (classId < 1)
                return ApiResponse<string>(errors: "Please provide valid class Id");

            try
            {
                var result = await _studentService.GetAllStudentsInClass(vM,classId);
                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("{Id}")]
        [RequiresPermission(Permission.STUDENT_UPDATE)]
        [ProducesResponseType(typeof(ApiResponse<StudentVM>), 200)]
        public async Task<IActionResult> UpdateStudent([FromForm]StudentUpdateVM vM, [FromRoute] long Id)
        {
            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            if (Id < 1)
            {
                return ApiResponse<string>(errors: "Id is invalid", codes: ApiResponseCodes.INVALID_REQUEST);
            }

            try
            {
                var result = await _studentService.UpdateStudent(Id, vM);
                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }


        [HttpDelete("{id}")]
        [RequiresPermission(Permission.STUDENT_DELETE)]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        public async Task<IActionResult> DeleteStudent(long id)
        {
            if ( id <1)
                return ApiResponse<string>(errors: "Invalid student Id");

            try
            {
                var result = await _studentService.DeleteStudent(id);
                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<bool>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

    }
}
