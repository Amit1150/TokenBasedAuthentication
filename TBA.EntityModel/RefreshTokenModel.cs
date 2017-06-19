using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBA.Utils;

namespace TBA.EntityModel
{
    public class RefreshTokenModel
    {
       
        public string DeviceId { get; set; }

       
        public DeviceType DeviceType { get; set; }

      
        public string RefreshToken { get; set; }
    }
}
