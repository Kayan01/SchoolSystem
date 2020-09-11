using LearningSvc.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels
{
    public class NoticeVM
    {
        public long Id { get; set; }
        public string Description { get; set; }

        public static implicit operator NoticeVM(Notice model)
        {
            return model == null ? null : new NoticeVM
            {
                Id = model.Id,
                Description = model.Description
            };
        }
    }
}
