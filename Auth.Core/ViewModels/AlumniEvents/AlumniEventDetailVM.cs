using Auth.Core.Models.Alumni;
using ExcelManager;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Extensions;
using Shared.Entities;
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
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public bool Status { get; set; }
        public List<string> EventTags { get; set; }

        public byte[] Image => EventImage.Path.GetBase64StringFromImage();

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public FileUpload EventImage { get; set; }

        public static implicit operator AlumniEventDetailVM(Models.Alumni.AlumniEvent model)
        {
            return model?.SetObjectProperty(new AlumniEventDetailVM());
        }

    }   
}
