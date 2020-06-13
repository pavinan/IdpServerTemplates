using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdpServer.ConfigModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IdpServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppConfig _appConfig;
        public HomeController(IOptions<AppConfig> options)
        {
            _appConfig = options.Value;
        }

        public IActionResult Index() => Redirect(_appConfig.ClientUrls.DefaultClientUrl);
    }
}
