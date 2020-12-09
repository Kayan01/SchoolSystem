using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.ViewModels.AssessmentSetup
{
    public class AssessmentSetupVM
    {
        public long Id { get; set; }
        public int SequenceNumber { get; set; }
        public string Name { get; set; }
        public int MaxScore { get; set; }

        public static implicit operator AssessmentSetupVM(Models.AssessmentSetup model)
        {
            return model == null ? null : new AssessmentSetupVM
            {
                Id = model.Id,
                SequenceNumber = model.SequenceNumber,
                Name = model.Name,
                MaxScore = model.MaxScore
            };
        }
    }
}
