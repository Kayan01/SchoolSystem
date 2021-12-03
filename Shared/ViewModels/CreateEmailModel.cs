using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Shared.ViewModels
{
    public class CreateEmailModel
    {
        public CreateEmailModel()
        {

        }
        public CreateEmailModel(string templateType, Dictionary<string, string> replacementDictionary, UserVM user, IEnumerable<string> attachments = null)
        {
            EmailTemplateType = templateType;
            ReplacementData = replacementDictionary;
            User = user;
            Attachments = attachments;
        }
        public CreateEmailModel(string templateType, Dictionary<string, string> replacementDictionary, UserVM user, string senderName, string emailPassword, IEnumerable<string> attachments = null)
        {
            EmailTemplateType = templateType;
            ReplacementData = replacementDictionary;
            User = user;
            Attachments = attachments;
            SenderName = senderName;
            EmailPassword = emailPassword;

        }

        public string EmailTemplateType { get; set; }
        public Dictionary<string, string> ReplacementData { get; set; }
        public UserVM User { get; set; }
        public IEnumerable<string> Attachments { get; set; }
        public string SenderName { get; set; }
        public string EmailPassword { get; set; }
    }
}
