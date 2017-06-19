using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;

namespace TBA.Utils
{
    public class CustomDateTimeSecondConverter : IsoDateTimeConverter
    {
        public CustomDateTimeSecondConverter()
        {
            base.DateTimeFormat = "dd-MMM-yyyy, HH:mm:ss";
        }
    }

    public class CustomDateConverter : IsoDateTimeConverter
    {
        public CustomDateConverter()
        {
            base.DateTimeFormat = "dd-MMM-yyyy";
        }
    }

    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            base.DateTimeFormat = "dd-MMM-yyyy, HH:mm";
        }
    }
}
