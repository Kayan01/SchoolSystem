using ExcelManager;

namespace Auth.Core.ViewModels.Staff
{
    public class ContactDetailsVM
    {
        [ExcelReaderCell]

        public string PhoneNumber { get; set; }
        [ExcelReaderCell]

        public string AltPhoneNumber { get; set; }
        [ExcelReaderCell]

        public string EmailAddress { get; set; }
        [ExcelReaderCell]

        public string AltEmailAddress { get; set; }
        [ExcelReaderCell]

        public string Country { get; set; }
        [ExcelReaderCell]

        public string Address { get; set; }
        [ExcelReaderCell]

        public string State { get; set; }
        [ExcelReaderCell]

        public string Town { get; set; }
    }
}
