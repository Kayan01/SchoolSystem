using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models.Alumni
{
    public class Alumni : Person 
    {
        public Alumni()
        {

        }

        public Alumni(Student student, string sessionName)
        {
            DateOfBirth = student.DateOfBirth;
            Address = student.Address;
            AdmissionDate = student.AdmissionDate;
            Country = student.Country;
            Level = student.Level;
            LocalGovernment = student.LocalGovernment;
            MothersMaidenName = student.MothersMaidenName;
            State = student.State;
            TenantId = student.TenantId;
            StudentType = student.StudentType;
            StudentId = student.Id;
            RegNumber = student.RegNumber;
            Nationality = student.Nationality;
            ParentId = student.ParentId.Value;
            Sex = student.Sex;
            Religion = student.Religion;
            StateOfOrigin = student.StateOfOrigin;
            SessionName = sessionName;
        }


        public long StudentId { get; set; }
        public string MothersMaidenName { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public string StateOfOrigin { get; set; }
        public string LocalGovernment { get; set; }
        public string TransportRoute { get; set; }
        public DateTime AdmissionDate { get; set; }
        public StudentType StudentType { get; set; }
        public string Level { get; set; }
        public string Country { get; set; }
        public string Town { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public long ParentId { get; set; }
        public long MedicalDetailID { get; set; }
        public string SessionName { get; set; }
        public string TermName { get; set; }
    }
}
