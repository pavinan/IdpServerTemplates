using IdpServer.Controllers.Consent;

namespace IdpServer.Controllers.Device
{
    public class DeviceAuthorizationInputModel : ConsentInputModel
    {
        public string UserCode { get; set; }
    }
}