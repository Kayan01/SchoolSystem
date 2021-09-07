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
namespace Auth.Core.ViewModels.AlumniEvent
{
    public class AlumniEventDetailVM
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Status { get; set; }
        public DateTime DateCreated { get; set; }
        public byte[] Image { get; set; }

        public static implicit operator AlumniEventDetailVM(Models.Alumni.AlumniEvent model)
        {
            return model == null ? null : model.SetObjectProperty(new AlumniEventDetailVM());
        }

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
