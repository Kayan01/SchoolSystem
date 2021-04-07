using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Auth.Core.Models;
using Auth.Core.ViewModels.School;

namespace Auth.Core.ViewModels
{
   public class UpdateSchoolVM : CreateSchoolVM
    {   

        public static implicit operator UpdateSchoolVM(Models.School model)
        {
            return model == null ? null : new UpdateSchoolVM
            {
                Name = model.Name
            };
        }

    }
}
