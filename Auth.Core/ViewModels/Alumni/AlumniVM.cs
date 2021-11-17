using Auth.Core.Models.Alumni;
using ExcelManager;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Extensions;
using Shared.Enums;
using Shared.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static Shared.Reflection.ReflectionHelper;
namespace Auth.Core.ViewModels
{
    public class AlumniDetailVM
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Status { get; set; }
        public DateTime DateCreated { get; set; }
        public byte[] Image { get; set; }
        public string Sex { get; set; }
        public string RegNumber { get; set; }
        public DateTime DateOfBirth { get; set; }


        public static implicit operator AlumniDetailVM(Models.Alumni.Alumni model)
        {
            return model == null ? null : model.SetObjectProperty(new AlumniDetailVM());
        }

    }

    public class AddAlumniExcelVM
    {
        [ExcelReaderCell()]
        public string Email { get; set; }
        [ExcelReaderCell()]
        public string FirstName { get; set; }
        [ExcelReaderCell()]
        public string LastName { get; set; }
        [ExcelReaderCell()]
        public string UserName { get; set; }

        [ExcelReaderCell()]
        public string PhoneNumber { get; set; }

        public List<DocumentType> DocumentTypes { get; set; }
        public List<IFormFile> Files { get; set; }
    }

    public class UpdateAlumniVM
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
    }


   
}
