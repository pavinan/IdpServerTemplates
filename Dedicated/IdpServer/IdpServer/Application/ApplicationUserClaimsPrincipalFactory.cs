using IdpServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdpServer.Application
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        public ApplicationUserClaimsPrincipalFactory(
            ApplicationUserManager userManager, 
            IOptions<IdentityOptions> optionsAccessor) 
            : base(userManager, optionsAccessor)
        {
        }
    }
}
