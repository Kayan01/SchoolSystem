using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Entities
{
    public class FileUpload : FullAuditedEntity<Guid>
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public string ContentType { get; set; }
    }
}
