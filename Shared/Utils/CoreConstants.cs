﻿using System.Collections.Generic;

namespace Shared.Utils
{
    public abstract class CoreConstants
    {
        public const string DateFormat = "dd MMMM, yyyy";
        public const string TimeFormat = "hh:mm tt";
        public const string SystemDateFormat = "dd/MM/yyyy";

        public static readonly string[] validExcels = new[] { ".xls", ".xlsx" };

        public static class EmailTemplateType
        {
            public const string PasswordReset = nameof(PasswordReset);
            public const string SuccessPasswordReset = nameof(SuccessPasswordReset);
            public const string NewUser = nameof(NewUser);
            public const string NewTeacher = nameof(NewTeacher);
        }

        public static class EntityType
        {
            public const string User = nameof(User);
            public const string Teacher = nameof(Teacher);
            public const string Student = nameof(Student);
            public const string Staff = nameof(Staff);
            public const string Assignment = nameof(Assignment);
        }

        public class EmailTemplate
        {
            public EmailTemplate(string name, string subject, string template)
            {
                Subject = subject;
                TemplatePath = template;
                Name = name;
            }
            public string Name { get; set; }
            public string Subject { get; set; }
            public string TemplatePath { get; set; }
        }

        public static readonly List<EmailTemplate> EmailTemplates = new List<EmailTemplate>
        {
            new EmailTemplate(EmailTemplateType.PasswordReset, "Password Reset Request", "filestore/emailtemplates/passwordreset.htm"),
            new EmailTemplate(EmailTemplateType.SuccessPasswordReset, "Successful Password Reset", "filestore/emailtemplates/successpasswordreset.htm"),
            new EmailTemplate(EmailTemplateType.NewUser, "New User", "filestore/emailtemplates/newuser.htm"),
            new EmailTemplate(EmailTemplateType.NewTeacher, "New Teacher", "filestore/emailtemplates/newteacher.htm"),
        };

        public class PaginationConsts
        {
            public const int PageSize = 25;
            public const int PageIndex = 1;
        }

        public class ClaimsKey
        {
            public const string LastLogin = nameof(LastLogin);
            public const string Division = nameof(Division);
            public const string Function = nameof(Function);
            public const string Grade = nameof(Grade);
            public const string Branch = nameof(Branch);
            public const string Directorate = nameof(Directorate);
            public const string Region = nameof(Region);
            public const string Unit = nameof(Unit);
            public const string JobCategory = nameof(JobCategory);
            public const string Permissions = nameof(Permissions);
        }

        public class AllowedFileExtensions
        {
            public const string Signature = ".jpg,.png";
        }

        public class JobFunction
        {
            public const string DH = "Divisional Head";
            public const string BM = "Branch Manager";
            public const string ED = "Executive Director";
            public const string MD = "Managing Director";
            public const string RBH = "Regional Bank Head";
            public const string BO = "Banking Officer";
            public const string CFO = "Chief Financial Officer";
        }

        public class Dashboard
        {

            public static string[] Months = new string[] {
            "Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
            };
        }

        public class DocumentType
        {
            public const string Invoice = "Invoice";
            public const string Contract = "Contract";
            public const string SignedContract = "SignedContract";
            public const string DeliveryNote = "DeliveryNote";
            public const string ProofOfItem = "ProofOfItem";
        }
    }
}