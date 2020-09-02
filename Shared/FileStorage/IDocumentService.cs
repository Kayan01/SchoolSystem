using Microsoft.AspNetCore.Http;
using Shared.Entities;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.FileStorage
{
   public interface IDocumentService
    {
        List<FileUpload> TryUploadSupportingDocuments(List<DocumentVM> formFiles);
    }
}
