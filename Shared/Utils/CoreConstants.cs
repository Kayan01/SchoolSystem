using System.Collections.Generic;

namespace Shared.Utils
{
    public abstract class CoreConstants
    {
        public const string DateFormat = "dd MMMM, yyyy";
        public const string TimeFormat = "hh:mm tt";
        public const string SystemDateFormat = "dd/MM/yyyy";

        public static readonly string[] validExcels = new[] { ".xls", ".xlsx" };

        public const string TestPdfTemplatePath1 = @"filestore/pdftemplate/TestPdfTemplate1.html";
        public const string TestPdfTemplatePath2 = @"filestore/pdftemplate/TestPdfTemplate2.html";
        public const string ResultPdfTemplatePath = @"pdftemplate/ResultTemplate.html";
        public const string InvoicePdfTemplatePath = @"pdftemplate/invoice-email-template.html";

        public static class EmailTemplateType
        {
            public const string PasswordReset = nameof(PasswordReset);
            public const string SuccessPasswordReset = nameof(SuccessPasswordReset);
            public const string NewUser = nameof(NewUser);
            public const string NewTeacher = nameof(NewTeacher);
            public const string NewSchool = nameof(NewSchool);
            public const string NewManager = nameof(NewManager);
            public const string AttendanceReport = nameof(AttendanceReport);
            public const string StudentResult = nameof(StudentResult);
            public const string Invoice = nameof(Invoice);
        }

        public static class EntityType
        {
            public const string School = nameof(School);
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
            new EmailTemplate(EmailTemplateType.PasswordReset, "Password Reset Request", "passwordreset.htm"),
            new EmailTemplate(EmailTemplateType.SuccessPasswordReset, "Successful Password Reset", "successpasswordreset.htm"),
            new EmailTemplate(EmailTemplateType.NewUser, "New User", "newuser.htm"),
            new EmailTemplate(EmailTemplateType.NewTeacher, "New Teacher", "newteacher.htm"),
            new EmailTemplate(EmailTemplateType.NewSchool, "New School", "newschool.htm"),
            new EmailTemplate(EmailTemplateType.NewManager, "New School", "newschoolmanager.htm"),
            new EmailTemplate(EmailTemplateType.StudentResult, "Student Result", "result.htm"),
            new EmailTemplate(EmailTemplateType.AttendanceReport, "Attendance Report", "attendance_alert.htm"),
            new EmailTemplate(EmailTemplateType.Invoice, "Invoice", "invoice.html"),
        };

        public class PaginationConsts
        {
            public const int PageSize = 25;
            public const int PageIndex = 1;
        }

        public class ClaimsKey
        {
            public const string LastLogin = nameof(LastLogin);
            public const string TenantId = nameof(TenantId);
            public const string UserType = nameof(UserType);
            public const string TeacherClassId = nameof(TeacherClassId);
            public const string StudentClassId = nameof(StudentClassId);
            public const string Permissions = nameof(Permissions);
            public const string SchGroupId = nameof(SchGroupId);
        }

        public class AllowedFileExtensions
        {
            public const string Signature = ".jpg,.png";
        }

        public class Dashboard
        {

            public static string[] Months = new string[] {
            "Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
            };
        }

        /*public class DocumentType
        {
            public const string Invoice = "Invoice";
            public const string Contract = "Contract";
            public const string SignedContract = "SignedContract";
            public const string DeliveryNote = "DeliveryNote";
            public const string ProofOfItem = "ProofOfItem";
        }*/
    }
}