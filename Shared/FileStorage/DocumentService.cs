using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Extensions;
using Shared.Entities;
using Shared.Enums;
using Shared.Utils;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.FileStorage
{
   public class DocumentService : IDocumentService
    {
        readonly IFileStorageService _fileStorageService;
        public DocumentService(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }
        public async Task<List<FileUpload>> TryUploadSupportingDocuments(List<IFormFile> formFiles, List<DocumentType> DocTypes)
        {

            var fileUploads = new List<FileUpload>();
            var uploaded = false;
            foreach (var file in formFiles)
            {
                var index = formFiles.IndexOf(file);
                var fileName = CommonHelper.GenerateTimeStampedFileName(file.FileName);
                uploaded = _fileStorageService.TrySaveStream(fileName,
                  file.OpenReadStream());
                if (uploaded)
                    fileUploads.Add(new FileUpload { ContentType = file.ContentType, Name = DocTypes[index].GetDisplayName(), Path = fileName });
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
