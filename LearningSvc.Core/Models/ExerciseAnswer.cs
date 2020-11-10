using Shared.Entities;
using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LearningSvc.Core.Models
{
    public class ExerciseAnswer : AuditedEntity<long>
    {
        //public long ClassSessionId { get; set; }
        //public long ExerciseId { get; set; }
        //public long StudentId { get; set; }
        //public string AnswerBody { get; set; }
        //[ForeignKey(nameof(FileUpload))]
        //public Guid? AnsweFileId { get; set; }
        //public string Comment { get; set; }
        //public double Score { get; set; }
        //public DateTime DateSubmitted { get; set; }


        //public FileUpload AnsweFile { get; set; }
        //public ClassSession ClassSession { get; set; }
        //public Exercise Exercise { get; set; }
        //public Student Student { get; set; }


    }
}
