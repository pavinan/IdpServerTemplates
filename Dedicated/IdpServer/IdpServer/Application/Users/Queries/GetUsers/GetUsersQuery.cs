using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdpServer.Application.Users.Queries.GetUsers
{
    public class GetUsersQuery : IRequest<IQueryable<ApplicationUserModel>>
    {
    }
}
