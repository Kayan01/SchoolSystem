using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningSvc.API.ViewModel
{
    public class UploadedFileVm
    {
        public string Name { get; set; }

        public Guid FileUploadId { get; set; }

        public string ContentType { get; set; }

        public string Document { get; set; }
    }

    public class UploadedSignature : UploadedFileVm
    {

        public int Id { get; set; }

    }
}
