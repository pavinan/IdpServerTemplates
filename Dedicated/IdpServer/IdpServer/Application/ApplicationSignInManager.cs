using IdpServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdpServer.Application
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser>
    {
        public ApplicationSignInManager(
            ApplicationUserManager userManager,
            IHttpContextAccessor contextAccessor, 
            ApplicationUserClaimsPrincipalFactory claimsFactory,
            IOptions<IdentityOptions> optionsAccessor, 
            ILogger<ApplicationSignInManager> logger,
            IAuthenticationSchemeProvider schemes, 
            IUserConfirmation<ApplicationUser> confirmation) 
            : base(
                  userManager, 
                  contextAccessor, claimsFactory,
                  optionsAccessor, 
                  logger, 
                  schemes, 
                  confirmation)
        {
        }
    }
}
