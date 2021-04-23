using System.Collections.Generic;
using System.IO;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;

namespace Shared.Utils
{
    public static class PdfHelper
    {
        public static byte[] BuildPdfFile(IConverter converter, IWebHostEnvironment hostingEnvironment, string pdfTemplatePath, object data, bool isLanddcape = false)
        {
            return converter.ConvertToPDFBytes(data, Path.Combine(hostingEnvironment.ContentRootPath, pdfTemplatePath), isLanddcape);
        }

        public static byte[] BuildPdfFile(IConverter converter, IWebHostEnvironment hostingEnvironment, string pdfTemplatePath, List<object> data, bool isLandscape = false)
        {
            return converter.ConvertToPDFBytesToList(data, Path.Combine(hostingEnvironment.ContentRootPath, pdfTemplatePath), isLandscape);
        }

        public static byte[] BuildPdfFile(IConverter converter, IWebHostEnvironment hostingEnvironment, string pdfTemplatePath, object mainData, List<object> tableData, TableAttributeConfig tableConfig, bool isLandscape = false)
        {
            return converter.ConvertToPDFBytesToList(mainData, tableData, tableConfig, Path.Combine(hostingEnvironment.ContentRootPath, pdfTemplatePath), isLandscape);
        }

        public static byte[] BuildPdfFile(IConverter converter, IWebHostEnvironment hostingEnvironment, string pdfTemplatePath, object mainData, IEnumerable<TableObject<object>> tableObj, IEnumerable<KeyValuePair<string, IEnumerable<TableObject<object>>>> tableArrays, bool isLandscape = false)
        {
            return converter.ConvertToPDFBytesToList(mainData, tableObj, tableArrays, Path.Combine(hostingEnvironment.ContentRootPath, pdfTemplatePath), isLandscape);
        }
    }
}
