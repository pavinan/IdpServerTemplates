using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdpServer.Application.Users.Queries.GetUsers;
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
        public async Task<SingleResult<ApplicationUserModel>> Me()
        {
            var userId = this.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => x.Value).Single();

            var usersQuery = await Mediator.Send(new GetUsersQuery());

            usersQuery = usersQuery.Where(x => x.Id == userId);

            var result = new SingleResult<ApplicationUserModel>(usersQuery);

            return result;
        }

    }
}
