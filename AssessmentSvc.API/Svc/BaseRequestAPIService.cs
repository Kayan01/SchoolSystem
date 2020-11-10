using Microsoft.AspNetCore.Http;
using Shared.AspNetCore;
using Shared.FileStorage;
using Shared.Utils;
using AssessmentSvc.API.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace AssessmentSvc.API.Svc
{
    public interface IBaseRequestAPIService
    {
        List<ValidationResult> ValidateBaseRequest(BaseRequestVM model, bool hasSupportingDocs = true);

        List<string> TryUploadDocuments(List<IFormFile> formFiles);

        string ConvertFileToBase64(string path);

        string ConvertFileToBase64(IFormFile file);
    }

    public class BaseRequestAPIService : IBaseRequestAPIService
    {
        private readonly IHttpUserService _httpUserService;
        private readonly IFileStorageService _fileStorageService;

        public BaseRequestAPIService(IHttpUserService httpUserService, IFileStorageService fileStorageService)
        {
            _httpUserService = httpUserService;
            _fileStorageService = fileStorageService;
        }

        public List<ValidationResult> ValidateBaseRequest(BaseRequestVM model, bool hasSupportingDocs = true)
        {
            var result = new List<ValidationResult>();

            return result;
        }

        public List<string> TryUploadDocuments(List<IFormFile> formFiles)
        {
            var filePaths = new List<string>();
            var uploaded = false;
            foreach (var file in formFiles)
            {
                var fileName = CommonHelper.GenerateTimeStampedFileName(file.FileName);
                uploaded = _fileStorageService.TrySaveStream(fileName,
                  file.OpenReadStream());
                if (uploaded)
                    filePaths.Add(fileName);
                else
                    break;
            }

            if (!uploaded && filePaths.Count() > 0)
            {
                foreach (var file in filePaths)
                    _fileStorageService.DeleteFile(file);

                filePaths.Clear();
            }
            return filePaths;
        }

        public string ConvertFileToBase64(string path)
        {
            var file = _fileStorageService.GetFile(path);
            string base64String;
            using (var fileStream = file.CreateReadStream())
            {
                using (var ms = new MemoryStream())
                {
                    fileStream.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    base64String = Convert.ToBase64String(fileBytes);
                }
            }
            return base64String;
        }

        public string ConvertFileToBase64(IFormFile file)
        {
            string base64String;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                base64String = Convert.ToBase64String(fileBytes);
            }
            return base64String;
        }
    }
}