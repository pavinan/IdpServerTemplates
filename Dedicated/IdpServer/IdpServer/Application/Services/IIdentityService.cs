using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdpServer.Application.Services
{
    public interface IIdentityService
    {
        public string GetUserId();
    }
}
