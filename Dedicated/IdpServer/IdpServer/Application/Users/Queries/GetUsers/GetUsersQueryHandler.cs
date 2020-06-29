using AutoMapper;
using AutoMapper.QueryableExtensions;
using IdpServer.Application.Services;
using IdpServer.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdpServer.Application.Users.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IQueryable<ApplicationUserModel>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(
            ApplicationDbContext dbContext,
            IIdentityService identityService,
            IMapper mapper)
        {
            this._dbContext = dbContext;
            this._identityService = identityService;
            this._mapper = mapper;
        }

        public Task<IQueryable<ApplicationUserModel>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = _dbContext.Users.AsNoTracking()
                .ProjectTo<ApplicationUserModel>(_mapper.ConfigurationProvider);

            return Task.FromResult(users);
        }
    }
}
