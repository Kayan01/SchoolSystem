using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Services.Interfaces
{
   public interface IAuthUserManagement
    {
        //adds user and returns id
        Task<int?> AddUserAsync(string firstName, string lastName, string email, string phoneNumber, string pwd);
       Task< bool> UpdateUserAsync(int id, string firstName, string lastName);
        Task<bool> DeleteUserAsync(int id);

    }
}
