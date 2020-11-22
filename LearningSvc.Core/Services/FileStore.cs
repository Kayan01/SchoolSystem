using LearningSvc.Core.Interfaces;
using Shared.DataAccess.Repository;
using Shared.Entities;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LearningSvc.Core.Services
{
    public class FileStore : IFileStore
    {
        private readonly IRepository<FileUpload, Guid> _fileRepo;
        public FileStore(IRepository<FileUpload, Guid> fileRepo)
        {
            _fileRepo = fileRepo;
        }
        public async Task<ResultModel<byte[]>> GetFile(Guid Id)
        {
            var result = new ResultModel<byte[]>();
            var file = await _fileRepo.FirstOrDefaultAsync(Id);


            if (file == null)
            {
                result.AddError("No file found");
                return result;
            }
            else
            {

                var filepath = Path.Combine("Filestore", file.Path);

                if (File.Exists(filepath))
                {
                    result.Data = File.ReadAllBytes(filepath);
                    return result;
                }
                else
                {
                    result.AddError("No file found");
                    return result;
                }
            }

        }

    }
}