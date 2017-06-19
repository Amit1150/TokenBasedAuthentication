using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBA.EntityModel
{
   public class ApiResponse
    {
        public ApiResponse()
        {
            Message = "Some error occurred. Please try again later.";
        }
        public string Message { get; set; }
    }
}
