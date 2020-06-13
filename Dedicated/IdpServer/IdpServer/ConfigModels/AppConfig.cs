using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdpServer.ConfigModels
{
    public class AppConfig
    {
        public ClientUrls ClientUrls { get; set; }
    }

    public class ClientUrls
    {
        public string DefaultClientUrl { get; set; }
        public string Angular { get; set; }
    }
}
