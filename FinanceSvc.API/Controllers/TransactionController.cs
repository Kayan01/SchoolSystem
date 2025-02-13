﻿using FinanceSvc.Core.Services.Interfaces;
using FinanceSvc.Core.ViewModels.Transaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.AspNetCore;
using Shared.AspNetCore.Policy;
using Shared.Permissions;
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
    public class TransactionController : BaseController
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> NewTransaction([FromBody] TransactionPostVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _transactionService.NewPendingTransaction(model);

                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<string>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<TransactionDetailsVM>), 200)]
        public async Task<IActionResult> GetTransaction(int id)
        {
            try
            {
                var result = await _transactionService.GetTransaction(id);
                if (result.HasError)
                    return ApiResponse<TransactionDetailsVM>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<TransactionVM>>), 200)]
        public async Task<IActionResult> GetAllAwaitingApprovalTransactions()
        {
            try
            {
                var result = await _transactionService.GetAllAwaitingApprovalTransactions();
                if (result.HasError)
                    return ApiResponse<List<TransactionVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.Data.Count);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<TransactionVM>>), 200)]
        public async Task<IActionResult> GetAllPendingTransactions([FromQuery]long? studentId)
        {
            try
            {
                var result = await _transactionService.GetAllPendingTransactions(studentId);
                if (result.HasError)
                    return ApiResponse<List<TransactionVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.Data.Count);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<TransactionVM>>), 200)]
        public async Task<IActionResult> GetAllTransactions([FromQuery]QueryModel query)
        {
            try
            {
                var result = await _transactionService.GetAllTransactions(query);
                if (result.HasError)
                    return ApiResponse<List<TransactionVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items, totalCount: result.Data.TotalItemCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{studentId}")]
        [ProducesResponseType(typeof(ApiResponse<List<TransactionVM>>), 200)]
        public async Task<IActionResult> GetTransactionHistory(long studentId)
        {
            try
            {
                var result = await _transactionService.GetTransactionHistory(studentId);
                if (result.HasError)
                    return ApiResponse<List<TransactionVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.Data.Count);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> UploadTransactionReceipt([FromForm] TransactionReceiptVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _transactionService.UploadTransactionReceipt(model);

                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<string>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> ApproveRejectTransaction([FromBody] TransactionApprovalVM model)
        {
            if (model == null)
                return ApiResponse<string>(errors: "Empty payload");

            if (!ModelState.IsValid)
                return ApiResponse<string>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);

            try
            {
                var result = await _transactionService.ApproveRejectTransaction(model);

                if (result.HasError)
                    return ApiResponse<string>(errors: result.ErrorMessages.ToArray());
                return ApiResponse<string>(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<ExportPayloadVM>), 200)]
        public async Task<IActionResult> ExportInvoiceReportExcel([FromQuery]TransStatus model)
        {
            var result = new ResultModel<ExportPayloadVM>();

            if (model ==  null)
            {
                return ApiResponse<List<TransactionVM>>(message : "Status field cannot be empty", errors: result.ErrorMessages.ToArray());
            }

            try
            {
                var data = await _transactionService.GetAllTransactionReportByStatus(model);
                if (data == null)
                {
                    return ApiResponse(message: "No Invoice record", codes: ApiResponseCodes.OK, data: result.Data, totalCount: data.Data.Count);
                }
                
                result = await _transactionService.ExportTransactionRecordExcel(data.Data);
                if (result.HasError)
                    return ApiResponse<List<TransactionVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: data.Data.Count);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<ExportPayloadVM>), 200)]
        public async Task<IActionResult> ExportInvoiceReportPDF([FromQuery]TransStatus model)
        {
            var result = new ResultModel<ExportPayloadVM>();

            if (model ==  null)
            {
                return ApiResponse<List<TransactionVM>>(message: "Status field cannot be empty", errors: result.ErrorMessages.ToArray());
            }

            try
            {
                var data = await _transactionService.GetAllTransactionReportByStatus(model);
                if (data == null)
                {
                    return ApiResponse(message: "No Invoice record", codes: ApiResponseCodes.OK, data: result.Data, totalCount: data.Data.Count);
                }

                result = await _transactionService.ExportTransactionRecordPDF(data.Data);
                if (result.HasError)
                    return ApiResponse<List<TransactionVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data, totalCount: data.Data.Count);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<TransactionVM>>), 200)]
        public async Task<IActionResult> GetAllTransactionsByStatus([FromQuery] QueryModel query,[FromQuery] TransStatus model)
        {
            try
            {
                var result = await _transactionService.ViewAllTransactionReportByStatus(model,query);
                if (result.HasError)
                    return ApiResponse<List<TransactionVM>>(errors: result.ErrorMessages.ToArray());
                return ApiResponse(message: "Successful", codes: ApiResponseCodes.OK, data: result.Data.Items, totalCount: result.Data.TotalItemCount);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
