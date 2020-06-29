using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdpServer.Models;
using IdpServer.Persistence;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdpServer.ApiControllers
{
    
    public class UsersController : BaseApiController
    {
        private readonly ApplicationDbContext _dbContext;

        public UsersController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Route("me")]
        [HttpGet]
        [EnableQuery]
        public SingleResult<ApplicationUser> Me()
        {
            var userId = this.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x=>x.Value).Single();

            var userQuery = _dbContext.Users.Where(x => x.Id == userId);

            var result = new SingleResult<ApplicationUser>(userQuery);

            return result;
        }

    }
}
