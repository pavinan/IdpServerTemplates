using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdpServer.Models
{
    public class ApplicationRole : IdentityRole<string>
    {
        public string DisplaName { get; set; }
    }
}
