using Newtonsoft.Json;
using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace FinanceSvc.Core.Models
{
    public class SessionSetup : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }

        public string SessionName { get; set; }
        public bool IsCurrent { get; set; }

        public string TermsJSON { get; set; } //will contain List of Term object as JSON string

        [NotMapped]
        public List<Term> Terms
        {
            get
            {
                return JsonConvert.DeserializeObject<List<Term>>(TermsJSON, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
        }
    }

    public class Term
    {
        public int SequenceNumber { get; set; }
        public string Name { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }


        public static string GetTermFromString (string json, int sequence)
        {
            try
            {
                var terms = JsonConvert.DeserializeObject<List<Term>>(json, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                return terms?.FirstOrDefault(m => m.SequenceNumber == sequence)?.Name;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
