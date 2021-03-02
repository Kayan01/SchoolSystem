using FinanceSvc.Core.Services.Interfaces;
using FinanceSvc.Core.ViewModels.Invoice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.ViewModels;
using Shared.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceSvc.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class InvoiceController : BaseController
    {
        private readonly IInvoiceService _invoiceService;
        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }


        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<InvoiceVM>>), 200)]
        public async Task<IActionResult> GetAllInvoices(string session, string term)
        {
            try
            {
                var result = await _invoiceService.GetAllInvoices(session, term);
                if (result.HasError)
                    return ApiResponse<List<InvoiceVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.Data.Count);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<InvoicePaymentVM>>), 200)]
        public async Task<IActionResult> GetInvoices([FromQuery]InvoiceRequestVM vm)
        { 
            try
            {
                var result = await _invoiceService.GetInvoices(vm);
                if (result.HasError)
                    return ApiResponse<List<InvoicePaymentVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.Data.Count);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<InvoicePaymentHistoryVM>>), 200)]
        public async Task<IActionResult> GetPaymentHistoryInvoices(string session, string term)
        {
            try
            {
                var result = await _invoiceService.GetPaymentHistoryInvoices(session, term);
                if (result.HasError)
                    return ApiResponse<List<InvoicePaymentHistoryVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.Data.Count);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<InvoicePaymentVM>>), 200)]
        public async Task<IActionResult> GetPaymentInvoices(string session, string term)
        {
            try
            {
                var result = await _invoiceService.GetPaymentInvoices(session, term);
                if (result.HasError)
                    return ApiResponse<List<InvoicePaymentVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.Data.Count);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<InvoicePendingPaymentVM>>), 200)]
        public async Task<IActionResult> GetPendingPaymentInvoices(string session, string term)
        {
            try
            {
                var result = await _invoiceService.GetPendingPaymentInvoices(session, term);
                if (result.HasError)
                    return ApiResponse<List<InvoicePendingPaymentVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.Data.Count);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<InvoiceDetailVM>), 200)]
        public async Task<IActionResult> GetInvoice(int id)
        {
            try
            {
                var result = await _invoiceService.GetInvoice(id);
                if (result.HasError)
                    return ApiResponse<InvoiceDetailVM>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> GenerateInvoice([FromBody] InvoicePostVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _invoiceService.AddInvoice(model);

                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<string>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

    }
}
