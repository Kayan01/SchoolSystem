using Microsoft.AspNetCore.Http;
using Shared.Entities;
using Shared.Enums;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.FileStorage
{
   public interface IDocumentService
    {
       Task< List<FileUpload>> TryUploadSupportingDocuments(List<IFormFile> formFiles, List<DocumentType> DocTypess);
       Task<FileUpload> TryUploadSupportingDocument(IFormFile formFile, DocumentType DocType);

        string TryGetUploadedFile(string path);
        string TryGetUploadedFileSize(string path);
    }
}
