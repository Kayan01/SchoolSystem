using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Auth.Core.ViewModels.Parent
{
    public class AddParentVM
    {
        [Required]
        public long StudentId { get; set; }
    }
}
