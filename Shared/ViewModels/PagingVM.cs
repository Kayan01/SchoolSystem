using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.ViewModels
{
    public class PagingVM
    {
        [Range(minimum: 0, maximum: double.MaxValue, ErrorMessage ="The field {0} must be greater than {1}")]
        public int PageNumber { get; set; }
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}")]
        public int PageSize { get; set; }
    }
}
