using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdpServer.ApiControllers
{
    [Route("v1.0/[controller]")]
    [Authorize("api")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
