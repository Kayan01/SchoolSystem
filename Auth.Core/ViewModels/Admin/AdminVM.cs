﻿using Auth.Core.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Extensions;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Auth.Core.ViewModels
{
    public class AdminVM
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? ImageId { get; set; }

        public static implicit operator AdminVM(Admin model)
        {
            return model == null ? null : new AdminVM
            {
                Id = model.Id,
                Email = model.User.Email,
                FirstName = model.User.FirstName,
                LastName = model.User.LastName,
                PhoneNumber = model.User?.PhoneNumber,
                UserName = model.User?.UserName,
                ImageId = model.FileUploads.FirstOrDefault(x => x.Name == DocumentType.ProfilePhoto.GetDisplayName())?.Id
            };
        }

    }

    public class AddAdminVM
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
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
