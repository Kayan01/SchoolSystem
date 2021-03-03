using Newtonsoft.Json;
using Shared.Entities.Auditing;
using Shared.Tenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AssessmentSvc.Core.Models
{
    public class Result : AuditedEntity<long>, ITenantModelType
    {
        public long TenantId { get; set; }

        public long StudentId { get; set; }
        public long SessionSetupId { get; set; }
        public long SchoolClassId { get; set; }
        public long SubjectId { get; set; }
        public int TermSequenceNumber { get; set; }
        public long? ApprovedResultId { get; set; }
        //will contain List of Scores for a course  as JSON string
        public string ScoresJSON { get; private set; }

        public ApprovedResult  ApprovedResult { get; set; }
        public Subject Subject { get; set; }
        public SchoolClass SchoolClass { get; set; }
        public Student Student { get; set; }
        public SessionSetup Session { get; set; }
        [NotMapped]
        public List<Score> Scores
        {
            get
            {
                return JsonConvert.DeserializeObject<List<Score>>(ScoresJSON, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            set
            {
                ScoresJSON = JsonConvert.SerializeObject(value, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
        }
    }

    public class Score
    {
        public string AssessmentName { get; set; }
        public double StudentScore { get; set; }
    }
}
