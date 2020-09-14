using Microsoft.OpenApi.Extensions;
using Shared.Entities;
using Shared.Utils;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared.FileStorage
{
   public class DocumentService : IDocumentService
    {
        readonly IFileStorageService _fileStorageService;
        public DocumentService(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }
        public List<FileUpload> TryUploadSupportingDocuments(List<DocumentVM> formFiles)
        {

            var fileUploads = new List<FileUpload>();
            var uploaded = false;
            foreach (var file in formFiles)
            {
                var formFile = file.File;
                var fileName = CommonHelper.GenerateTimeStampedFileName(formFile.FileName);
                uploaded = _fileStorageService.TrySaveStream(fileName,
                  formFile.OpenReadStream());
                if (uploaded)
                    fileUploads.Add(new FileUpload { ContentType = formFile.ContentType, Name = file.DocumentType.GetDisplayName(), Path = fileName });
                else
                    break;
            }

            if (!uploaded && fileUploads.Count() > 0)
            {
                foreach (var file in fileUploads)
                    _fileStorageService.DeleteFile(file.Path);
            }
            return fileUploads;
        }


    }
}
