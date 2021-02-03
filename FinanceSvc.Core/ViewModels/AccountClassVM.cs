using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceSvc.Core.ViewModels
{
    public class AccountClassVM
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public int MinNumberValue { get; set; }
        public int MaxNumberValue { get; set; }

        public bool IsActive { get; set; }


        public static implicit operator AccountClassVM(Models.AccountClass model)
        {
            return model == null ? null : new AccountClassVM
            {
                Id = model.Id,
                Name = model.Name,
                MinNumberValue = model.MinNumberValue,
                MaxNumberValue = model.MaxNumberValue,
                IsActive = model.IsActive,
            };
        }
    }
}
