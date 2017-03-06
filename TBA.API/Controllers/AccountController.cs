using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TBA.DataAccess.Interfaces;
using TBA.Model;

namespace TBA.API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAppUserRepository appUserRepository;
        private readonly ILog logger;
        public AccountController(IAppUserRepository appUserRepository, ILog logger)
            :base()
        {
            this.appUserRepository = appUserRepository;
            this.logger = logger;
        }

        [HttpGet]
        public IHttpActionResult Login()
        {
            try
            {
            }
            catch (Exception ex)
            {
               
            }
            return Ok();
        }
    }
}
