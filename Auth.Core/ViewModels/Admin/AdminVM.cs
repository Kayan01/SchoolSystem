using Auth.Core.Models.Users;
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

namespace Auth.Core.ViewModels
{
    public class AdminListVM
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Status { get; set; }
        public DateTime DateCreated { get; set; }
        public byte[] Image { get; set; }

        public static implicit operator AdminListVM(Admin model)
        {
            return model == null ? null : new AdminListVM
            {
                Id = model.Id,
                Email = model.User.Email,
                FirstName = model.User.FirstName,
                LastName = model.User.LastName,
                DateCreated = model.CreationTime,
                Image = model.FileUploads.FirstOrDefault(x => x.Name == DocumentType.ProfilePhoto.GetDisplayName())?.Path.GetBase64StringFromImage()
            };
        }

    }

    public class AdminDetailVM
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Status { get; set; }
        public DateTime DateCreated { get; set; }
        public byte[] Image { get; set; }

        public static implicit operator AdminDetailVM(Admin model)
        {
            return model == null ? null : new AdminDetailVM
            {
                Id = model.Id,
                Email = model.User.Email,
                FirstName = model.User.FirstName,
                LastName = model.User.LastName,
                DateCreated = model.CreationTime,
                Image = model.FileUploads.FirstOrDefault(x => x.Name == DocumentType.ProfilePhoto.GetDisplayName())?.Path.GetBase64StringFromImage()
            };
        }

    }

    public class AddAdminVM
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

    public class UpdateAdminVM
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
    }


   
}
