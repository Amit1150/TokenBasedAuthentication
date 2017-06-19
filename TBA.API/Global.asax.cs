using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Formatting;
using System.Web.Http;


namespace TBA.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
          
            GlobalConfiguration.Configuration.Formatters.Clear();
            GlobalConfiguration.Configuration.Formatters.Add(new JsonMediaTypeFormatter());
            GlobalConfiguration.Configuration
                            .Formatters
                            .JsonFormatter
                            .SerializerSettings
                            .NullValueHandling = NullValueHandling.Ignore;
            GlobalConfiguration.Configuration
                            .Formatters
                            .JsonFormatter
                            .SerializerSettings
                            .ContractResolver = new DefaultContractResolver();
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutofacConfig.RegisterComponents();
        }
    }
}
