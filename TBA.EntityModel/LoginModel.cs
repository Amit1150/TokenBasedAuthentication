using Newtonsoft.Json;
using TBA.Utils;

namespace TBA.EntityModel
{
    public class LoginModel
    {   

        public string DeviceId { get; set; }

        
        public DeviceType DeviceType { get; set; }

       
        public string UserName { get; set; }

       
        public string Password { get; set; }
    }
}
