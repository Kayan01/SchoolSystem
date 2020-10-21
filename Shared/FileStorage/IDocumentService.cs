﻿using Microsoft.AspNetCore.Http;
using Shared.Entities;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.FileStorage
{
   public interface IDocumentService
    {
       Task< List<FileUpload>> TryUploadSupportingDocuments(List<DocumentVM> formFiles);
    }
}
