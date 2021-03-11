using FinanceSvc.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.Repository;
using Shared.Entities;
using Shared.FileStorage;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceSvc.Core.Services
{
    public class FileStore : IFileStore
    {
        private readonly IRepository<FileUpload, Guid> _fileRepo;
        private readonly IDocumentService _documentService;
        public FileStore(IRepository<FileUpload, Guid> fileRepo,
            IDocumentService documentService)
        {
            _fileRepo = fileRepo;
            _documentService = documentService;
        }
        public async Task<ResultModel<string>> GetFile(Guid Id)
        {
            var file = await _fileRepo.GetAll().SingleOrDefaultAsync(m=> m.Id == Id);


            if (file == null)
            {
                return new ResultModel<string>("File not found");
            }

            var fileString = _documentService.TryGetUploadedFile(file.Path);

            return new ResultModel<string>(data: fileString);
        }

    }
}
