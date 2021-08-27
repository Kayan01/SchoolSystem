using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Interfaces
{
    public interface IPromotionService
    {
        public Task PromoteAllStudent(PromotionSharedModel model);
    }
}
