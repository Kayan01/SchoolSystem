﻿using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.Services.Interfaces
{
    public interface IParentService
    {
        void AddOrUpdateParentFromBroadcast(ParentSharedModel model);
    }
}
