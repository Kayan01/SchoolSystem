using DinkToPdf.Contracts;
using Shared.FileStorage;
using Shared.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.Utils
{
    public interface IToPDF
    {
        string ResultToPDF(object mainData, IEnumerable<TableObject<object>> tableObj, IEnumerable<KeyValuePair<string, IEnumerable<TableObject<object>>>> tableArrays, string htmlTemplatePath, bool isLandscape = false);
    }
        public class ToPDF : IToPDF
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IConverter _converter;
        public ToPDF(
            IFileStorageService fileStorageService,
            IConverter converter)
        {
            _fileStorageService = fileStorageService;
            _converter = converter;
        }

        public string ResultToPDF( object mainData, IEnumerable<TableObject<object>> tableObj, IEnumerable<KeyValuePair<string, IEnumerable<TableObject<object>>>> tableArrays, string htmlTemplatePath, bool isLandscape = false)
        {
            var pdf = _converter.ConvertToPDFBytesToList(mainData, tableObj, tableArrays, htmlTemplatePath, isLandscape);
            var path = $"result/{Guid.NewGuid()}.pdf";
            _fileStorageService.SaveBytes(path, pdf);

            return path;
        }

    }
}
