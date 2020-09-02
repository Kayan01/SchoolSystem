using Microsoft.AspNetCore.Http;
using Shared.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels
{
    public class DocumentVM
    {
        public DocumentType DocumentType { get; set; }
        public IFormFile File { get; set; }
    }
}
