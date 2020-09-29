﻿using Auth.Core.ViewModels;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Services.Interfaces
{
    public interface IAuthUserManagement
    {
        //adds user and returns id
        Task<long?> AddUserAsync(AuthUserModel model);

        Task<bool> UpdateUserAsync(long id, AuthUserModel model);

        Task<bool> DeleteUserAsync(long id);

        IQueryable<User> GetAllAuthUsersAsync();
    }
}