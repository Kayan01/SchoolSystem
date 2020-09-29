using Auth.Core.ViewModels;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Services.Interfaces
{
    public interface ITestService
    {
        Task<ResultModel<object>> GetTests();
        Task<ResultModel<string>> TestBroadcast(string title);
        Task<ResultModel<TestVM>> AddTest(TestVM model);
    }
}
