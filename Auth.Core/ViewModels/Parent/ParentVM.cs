﻿using Microsoft.OpenApi.Extensions;
using Shared.Enums;
using Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auth.Core.ViewModels.Parent
{
    public class ChildView
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public long Id { get; set; }
    }
    public class ParentDetailVM
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public string Occupation { get; set; }
        public string ModeOfIdentification { get; set; }
        public string IdentificationNumber { get; set; }
        public string ContactNumber { get; set; }
        public string ContactEmail { get; set; }
        public string ContactHomeAddress { get; set; }
        public string OfficeHomeAddress { get; set; }
        public string SecondaryPhoneNumber { get; set; }
        public string SecondaryEmailAddress { get; set; }
        public bool IsActive { get; set; }
        public List<ChildView> Children { get; set; }

        public static implicit operator ParentDetailVM(Models.Users.Parent model)
        {
            return model == null ? null : new ParentDetailVM
            {
                ContactEmail = model.User.Email,
                ContactHomeAddress = model.HomeAddress,
                ContactNumber = model.User.PhoneNumber,
                FirstName = model.User.FirstName,
                IdentificationNumber = model.IdentificationNumber,
                LastName = model.User.LastName,
                ModeOfIdentification = model.IdentificationType,
                Occupation = model.Occupation,
                OfficeHomeAddress = model.OfficeAddress,
                Sex = model.Sex,
                Title = model.Title,
                SecondaryEmailAddress = model.SecondaryEmail,
                SecondaryPhoneNumber = model.SecondaryPhoneNumber,
                IsActive = model.Status,
                Children = model.Students?.Select(x => new ChildView
                {
                    Id = x.Id,
                    Name = x.User?.FullName,
                    //Image = x.FileUploads.FirstOrDefault(x => x.Name == DocumentType.ProfilePhoto.GetDisplayName())?.Path.GetBase64StringFromImage()
                }).ToList()
            };
        }
    }

    public class ParentListVM
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string ParentCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public string Image { get; set; }
        public string HomeAddress { get; set; }
        public List<Models.Student> Student { get; set; }


        public static implicit operator ParentListVM(Models.Users.Parent model)
        {
            return model == null ? null : new ParentListVM
            {
                FullName = model.User.FullName,
                Id = model.Id,
                ParentCode = $"PRT/{model.CreationTime.Year}/{model.Id}",
                PhoneNumber = model.User.PhoneNumber,
                Email = model.User.Email,
                Status = model.Status,
                HomeAddress = model.HomeAddress,
            };
        }
    }

    public class StudentDT
    {
        public string StudentName { get; set; }
        public string ClassName { get; set; }
    }

    public class ParentListDetailVM
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string ParentCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public string Image { get; set; }
        public string HomeAddress { get; set; }
        public List<Models.Student> Student { get; set; }
        public List<StudentDT> ChildDetails { get; set; }
        public int TotalKidsInSchool { get; set; }




        public static implicit operator ParentListDetailVM(Models.Users.Parent model)
        {
            return model == null ? null : new ParentListDetailVM
            {
                FullName = model.User.FullName,
                Id = model.Id,
                ParentCode = $"PRT/{model.CreationTime.Year}/{model.Id}",
                PhoneNumber = model.User.PhoneNumber,
                Email = model.User.Email,
                Status = model.Status,
                HomeAddress = model.HomeAddress,
                Student = model.Students,
                ChildDetails = model.Students.Select(x => new StudentDT
                {
                    StudentName = x.User.FullName,
                    ClassName = x.Class.FullName
                }).ToList()
            };
        }
    }

}
