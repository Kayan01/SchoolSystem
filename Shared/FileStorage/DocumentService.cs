using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Extensions;
using Shared.Entities;
using Shared.Enums;
using Shared.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.FileStorage
{
    public class DocumentService : IDocumentService
    {
        readonly IFileStorageService _fileStorageService;
        private readonly ILogger<DocumentService> _logger;
        public DocumentService(IFileStorageService fileStorageService, ILogger<DocumentService> logger)
        {
            _fileStorageService = fileStorageService;
            _logger = logger;
        }

        public string TryGetUploadedFile(string path)
        {
            try
            {

                var file = _fileStorageService.GetFile(path);
                string base64String;
                using (var fileStream = file.CreateReadStream())
                {
                    using var ms = new MemoryStream();
                    fileStream.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    base64String = Convert.ToBase64String(fileBytes);
                }
                return base64String;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return null;
            }

        }

        public string TryGetUploadedFileSize(string path)
        {
            var file = _fileStorageService.GetFile(path);
            
            return $"{(file.Length/1000).ToString("0.00")} KB";

        }


        public async Task<FileUpload> TryUploadSupportingDocument(IFormFile formFile, DocumentType docType)
        {
            var fileName = CommonHelper.GenerateTimeStampedFileName(formFile.FileName);
            var uploaded = await _fileStorageService.TrySaveStreamAsync(fileName,
              formFile.OpenReadStream());

            if (uploaded)
                return new FileUpload { ContentType = formFile.ContentType, Name = docType.GetDisplayName(), Path = fileName };

            return null;
        }

        public async Task<List<FileUpload>> TryUploadSupportingDocuments(List<IFormFile> formFiles, List<DocumentType> DocTypes)
        {

            var fileUploads = new List<FileUpload>();
            var uploaded = false;
            foreach (var file in formFiles)
            {
                var index = formFiles.IndexOf(file);
                var fileName = CommonHelper.GenerateTimeStampedFileName(file.FileName);
                uploaded = await _fileStorageService.TrySaveStreamAsync(fileName,
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
