﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdpServer.ApiControllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        
    }
}
