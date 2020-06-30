using IdpServer.BuildingBlocks.AutoMapper;
using IdpServer.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdpServer.Application.Users.Commands.Update
{
    public class UpdateUserCommand: IRequest<string>, IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
