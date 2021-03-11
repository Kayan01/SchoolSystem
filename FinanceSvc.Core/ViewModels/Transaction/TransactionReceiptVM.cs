using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FinanceSvc.Core.ViewModels.Transaction
{
    public class TransactionReceiptVM
    {
        [Required]
        public long TransactionId { get; set; }
        [Required]
        public IFormFile Document { get; set; }
    }
}
