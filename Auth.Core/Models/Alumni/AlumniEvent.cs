using Newtonsoft.Json;
using Shared.Entities;
using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Auth.Core.Models.Alumni
{
    public  class AlumniEvent : FullAuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        private string DEventTags { get; set; }
        public FileUpload EventImage { get; set; }
        public bool Status { get; set; }

        [NotMapped]
        public List<string> EventTags
        {
            get
            {
                try
                {
                    return JsonConvert.DeserializeObject<List<string>>(DEventTags, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                }
                catch (Exception)
                {
                    return new List<string>();
                }

            }

            set
            {
                DEventTags = JsonConvert.SerializeObject(value, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
        }
    }
}
