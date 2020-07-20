using System.Collections.Generic;

namespace Shared.Utils
{
    public abstract class CoreConstants
    {
        public const string DateFormat = "dd MMMM, yyyy";
        public const string TimeFormat = "hh:mm tt";
        public const string SystemDateFormat = "dd/MM/yyyy";
        public const string SkuPrefix = "SKU-";
        public const int requestIdNoMinLength = 8;

        public static readonly string[] validExcels = new[] { ".xls", ".xlsx" };

        public class MailUrl
        {
            public const string PasswordReset = "filestore/emailtemplates/passwordreset.htm";
            public const string RFQInvitation = "RFQInvitation";
            public const string QuotationCreation = "QuotationCreation";
            public const string SelectedQuote = "SelectedQuotation";
            public const string ContractOffer = "ContactOffer";
            public const string SupplyInitialization = "SupplyInitialization";
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
            new EmailTemplate(MailUrl.RFQInvitation, "RFQ Invitation", "filestore/emailtemplates/rfqinvitation.htm"),
            new EmailTemplate(MailUrl.QuotationCreation, "QUotation Url", "filestore/emailtemplates/quotecreation.htm"),
            new EmailTemplate(MailUrl.SelectedQuote, "Selected Quotation", "filestore/emailtemplates/selectedquote.htm"),
            new EmailTemplate(MailUrl.ContractOffer, "Contract Offer", "filestore/emailtemplates/contractoffer.htm"),
            new EmailTemplate(MailUrl.SupplyInitialization, "Supply Initialization", "filestore/emailtemplates/supplyinit.htm" ),
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