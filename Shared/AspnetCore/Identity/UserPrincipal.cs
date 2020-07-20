using IdentityModel;
using Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Shared.AspnetCore.Identity
{
    public class UserPrincipal : ClaimsPrincipal
    {
        public UserPrincipal(ClaimsPrincipal principal) : base(principal)
        {
        }

        private string GetClaimValue(string key)
        {
            var identity = Identity as ClaimsIdentity;
            if (identity == null)
                return null;

            var claim = identity.Claims.FirstOrDefault(c => c.Type == key);
            return claim?.Value;
        }

        public string UserName
        {
            get
            {
                if (FindFirst(JwtClaimTypes.Id) == null)
                    return string.Empty;

                return GetClaimValue(JwtClaimTypes.Id);
            }
        }

        public string Email
        {
            get
            {
                if (FindFirst(JwtClaimTypes.Email) == null)
                    return string.Empty;

                return GetClaimValue(JwtClaimTypes.Email);
            }
        }

        public int UserId
        {
            get
            {
                if (FindFirst(JwtClaimTypes.Subject) == null)
                    return default;

                return Convert.ToInt32(GetClaimValue(JwtClaimTypes.Subject));
            }
        }

        public string FirstName
        {
            get
            {
                var usernameClaim = FindFirst(JwtClaimTypes.GivenName);

                if (usernameClaim == null)
                    return string.Empty;

                return usernameClaim.Value;
            }
        }

        public string LastName
        {
            get
            {
                var usernameClaim = FindFirst(JwtClaimTypes.FamilyName);

                if (usernameClaim == null)
                    return string.Empty;

                return usernameClaim.Value;
            }
        }

        public string Function
        {
            get
            {
                var functionClaim = FindFirst(CoreConstants.ClaimsKey.Function);
                if (functionClaim is null)
                    return string.Empty;
                return functionClaim.Value;
            }
        }

        public string Category
        {
            get
            {
                var categoryClaim = FindFirst(CoreConstants.ClaimsKey.JobCategory);
                if (categoryClaim is null)
                    return string.Empty;
                return categoryClaim.Value;
            }
        }


        public string Directorate
        {
            get
            {
                var directorateClaim = FindFirst(CoreConstants.ClaimsKey.Directorate);
                if (directorateClaim is null)
                    return string.Empty;
                return directorateClaim.Value;
            }
        }

        public string Region
        {
            get
            {
                var regionClaim = FindFirst(CoreConstants.ClaimsKey.Region);
                if (regionClaim is null)
                    return string.Empty;
                return regionClaim.Value;
            }
        }

        public string Division
        {
            get
            {
                var divisionClaim = FindFirst(CoreConstants.ClaimsKey.Division);
                if (divisionClaim is null)
                    return string.Empty;
                return divisionClaim.Value;
            }
        }

        //public string JobCategory
        //{
        //    get
        //    {
        //        var functionClaim = FindFirst(CoreConstants.ClaimsKey.JobCategory);
        //        if (functionClaim is null)
        //            return string.Empty;
        //        return functionClaim.Value;
        //    }
        //}

        public IEnumerable<string> Branches
        {
            get
            {
                var functionClaim = FindAll(x => x.Type.Equals(CoreConstants.ClaimsKey.Branch, StringComparison.InvariantCultureIgnoreCase));
                if (functionClaim is null)
                    return new List<string>();

                return functionClaim.Select(x => x.Value);
            }
        }

        // TODO: Add get branch Id from claims
        public int BranchId
        {
            get
            {
                return 1;
            }
        }
    }
}