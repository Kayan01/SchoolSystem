﻿using Microsoft.AspNetCore.Http;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ExcelManager;

namespace Auth.Core.ViewModels.Student
{
    public class CreateStudentVM
    {
        [ExcelReaderCell]
        public string FirstName { get; set; }
        [ExcelReaderCell]
        public string LastName { get; set; }
        [ExcelReaderCell]
        public string OtherNames { get; set; }
        [ExcelReaderCell]
        public string MothersMaidenName { get; set; }
        [ExcelReaderCell]
        public string Sex { get; set; }
        [ExcelReaderCell]
        public DateTime DateOfBirth { get; set; }
        [ExcelReaderCell]
        public string Religion { get; set; }
        [ExcelReaderCell]
        public string Nationality { get; set; }
        [ExcelReaderCell]
        public string StateOfOrigin { get; set; }
        [ExcelReaderCell]
        public string LocalGovt { get; set; }
        [ExcelReaderCell]
        public string TransportRoute { get; set; }
        [ExcelReaderCell]
        public string EntryType { get; set; }
        [ExcelReaderCell]
        public DateTime AdmissionDate { get; set; }
        [Required]
        [ExcelReaderCell]
        public long SectionId { get; set; }
        [Required]
        [ExcelReaderCell]
        public long ClassId { get; set; }
        [ExcelReaderCell]
        public StudentType StudentType { get; set; }
        [ExcelReaderCell]
        public string Level { get; set; }
        [ExcelReaderCell]
        public string ContactPhone { get; set; }
        [ExcelReaderCell]
        public string ContactCountry { get; set; }
        [ExcelReaderCell]
        public string ContactTown { get; set; }
        [ExcelReaderCell]
        public string ContactEmail { get; set; }
        [ExcelReaderCell]
        public string ContactAddress { get; set; }
        [ExcelReaderCell]
        public string ContactState { get; set; }
        [ExcelReaderCell]
        public string BloodGroup { get; set; }
        [ExcelReaderCell]
        public string Genotype { get; set; }
        public bool Disability { get; set; }
        [ExcelReaderCell]
        public string Allergies { get; set; }
        [ExcelReaderCell]
        public string ConfidentialNotes { get; set; }
        public List<ImmunizationVm> ImmunizationVms { get; set; } = new List<ImmunizationVm>();
        [Required]
        [ExcelReaderCell]
        public long ParentId { get; set; }

        public bool IsActive { get; set; } = true;

        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
        public List<DocumentType> DocumentTypes { get; set; } = new List<DocumentType>();
    }


    public class DeactivateStudent
    {
        public string SessionName { get; set; }
        
        public string DeactivationReason { get; set; }

    }

}
