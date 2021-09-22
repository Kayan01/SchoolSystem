using Auth.Core.Interfaces;
using Auth.Core.ViewModels.Subscription;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class SubscriptionInvoiceController : BaseController
    {
        private readonly ISubscriptionInvoiceService _invoiceService;

        public SubscriptionInvoiceController(ISubscriptionInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet("{schoolId}")]
        [ProducesResponseType(typeof(ApiResponse<SubcriptionInvoiceVM>), 200)]
        public async Task<IActionResult> GetNextSubsciptionInvoice(long schoolId)
        {
            try
            {
                var result = await _invoiceService.GetNextSubsciptionInvoice(schoolId);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{schoolId}")]
        [ProducesResponseType(typeof(ApiResponse<SubcriptionInvoiceVM>), 200)]
        public async Task<IActionResult> GetArrearsSubsciptionInvoice(long schoolId)
        {
            try
            {
                var result = await _invoiceService.GetArrearsSubsciptionInvoice(schoolId);
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
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> PostNextSubsciptionInvoice([FromBody] SubcriptionInvoiceVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<object>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _invoiceService.PostNextSubsciptionInvoice(model);
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
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> PostArrearsSubsciptionInvoice([FromBody] SubcriptionInvoiceVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<object>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _invoiceService.PostArrearsSubsciptionInvoice(model);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{schoolId}")]
        [ProducesResponseType(typeof(ApiResponse<SubcriptionInvoiceVM>), 200)]
        public async Task<IActionResult> GetUnpaidSubsciptionInvoice(long schoolId)
        {
            try
            {
                var result = await _invoiceService.GetUnpaidSubsciptionInvoice(schoolId);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("{invoiceId}")]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> MarkInvoiceAsPaid(long invoiceId)
        {
            try
            {
                var result = await _invoiceService.MarkInvoiceAsPaid(invoiceId);
                if (result.HasError)
                    return ApiResponse<object>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<object>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

    }
}
