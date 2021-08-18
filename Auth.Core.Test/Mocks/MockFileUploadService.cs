using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Extensions;
using Moq;
using Shared.Entities;
using Shared.Enums;
using Shared.FileStorage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Test.Mocks
{
    public class MockFileUploadService
    {
        public Mock<IDocumentService> Mock { get; }

        public MockFileUploadService()
        {
            Mock = new Mock<IDocumentService>();

            var FileMock = new Mock<IFormFile>();
            DocumentType doctype = new DocumentType();
            var file = new FileUpload() { 
                ContentType = FileMock.Object.ContentType,
                Name = doctype.GetDisplayName(),
                Path = FileMock.Object.FileName,
            };
            List<FileUpload> ListOfFiles = new List<FileUpload>();
            ListOfFiles.Add(file);

            Mock.Setup(m => m.TryGetUploadedFile(It.IsAny<string>())).Returns("file");
            Mock.Setup(m => m.TryGetUploadedFileSize(It.IsAny<string>())).Returns("file");
            Mock.Setup(m => m.TryUploadSupportingDocument(It.IsAny<IFormFile>(), It.IsAny<DocumentType>())).Returns(Task.FromResult(file));
            Mock.Setup(m => m.TryUploadSupportingDocuments(It.IsAny<List<IFormFile>>(), It.IsAny<List<DocumentType>>())).Returns(Task.FromResult(ListOfFiles));
        }
    }
}
