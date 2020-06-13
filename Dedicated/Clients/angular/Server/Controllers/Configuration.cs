
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Angular.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private AppConfig appConfig;

        public ConfigurationController(IOptions<AppConfig> appConfigOptions)
        {
            appConfig = appConfigOptions.Value;
        }

        [HttpGet]
        public AppConfig Get()
        {
            return appConfig;
        }
    }


    public class AppConfig
    {
        public string IdentityUrl { get; set; }
        public string AppUrl { get; set; }
    }
}
