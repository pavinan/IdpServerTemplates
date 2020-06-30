using AutoMapper;
using IdpServer.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdpServer.Application.Users.Commands.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, string>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(
            ApplicationDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<string> Handle(
            UpdateUserCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.SingleAsync(x => x.Id == request.Id, cancellationToken);
            _mapper.Map(request, user);
            user.LastModifiedDateTime = DateTimeOffset.UtcNow;

            await _dbContext.SaveChangesAsync();

            return user.Id;
        }
    }
}
