using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Exceptions
{
    public class TenantNotFoundException : Exception
    {
        public string ErrorCode { get; set; }

        public TenantNotFoundException(string message) : base(message)
        { }

        public TenantNotFoundException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
