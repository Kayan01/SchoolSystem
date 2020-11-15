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
        public CreateEmailModel(string templateType, StringDictionary replacementDictionary, UserVM user)
        {
            EmailTemplateType = templateType;
            ReplacementData = replacementDictionary;
            User = user;
        }

        public string EmailTemplateType { get; set; }
        public StringDictionary ReplacementData { get; set; }
        public UserVM User { get; set; }
    }
}
