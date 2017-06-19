using log4net;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Configuration;
using System.Security.Claims;
using Microsoft.Owin.Security.OAuth;
using TBA.DataAccess.Interfaces;
using TBA.EntityModel;
using TBA.Utils;
using TBA.Model;
using Microsoft.AspNet.Identity;

namespace TBA.API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAppUserRepository appUserRepository;
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly ILog logger;
        private ApplicationUserManager _userManager;

        public AccountController(IAppUserRepository appUserRepository, IRefreshTokenRepository refreshTokenRepository, 
            ILog logger, ApplicationUserManager userManager)
            : base()
        {
            this.appUserRepository = appUserRepository;
            this.refreshTokenRepository = refreshTokenRepository;
            this.logger = logger;
            _userManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        
        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }
     
        [HttpPost]
        public IHttpActionResult Register(AppUser user)
        {
            ApiResponse apiResponse = new ApiResponse();
            user.IsBlocked = false;
            user.PasswordHash = UserManager.PasswordHasher.HashPassword(user.PasswordHash);
            appUserRepository.AddUser(user);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, apiResponse));
        }
       
        [HttpPost]
        public IHttpActionResult Login(LoginModel loginModel)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    var getUser = appUserRepository.GetUserByUserName(loginModel.UserName);
                    if (getUser != null && UserManager.PasswordHasher.VerifyHashedPassword(getUser.PasswordHash,loginModel.Password) == PasswordVerificationResult.Success)
                    {
                        if (!getUser.IsBlocked)
                        {
                            var getUserRoles = appUserRepository.GetUserRoles(getUser.Id);
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, GenerateTokenResponse(getUser, getUserRoles, loginModel.DeviceId, loginModel.DeviceType)));
                        }
                        else
                        {
                            apiResponse.Message = "Your account has been blocked.";
                        }
                    }
                    else
                    {
                        apiResponse.Message = "Invalid username or password.";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, apiResponse));
        }

        /// <summary>
        /// Get Access token using refresh token.
        /// </summary>
        /// <param name="refreshTokenModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult GetAccessToken(RefreshTokenModel refreshTokenModel)
        {
            ApiResponse apiResponse = new ApiResponse();
            apiResponse.Message = "Your session has expired. Kindly login again.";
            try
            {
                var getHashToken = GenerateHash.GetHash(refreshTokenModel.RefreshToken);
                var getRefreshTokenDetails = refreshTokenRepository.GetRefreshTokenDetail(getHashToken);
                if (getRefreshTokenDetails != null && getRefreshTokenDetails.ExpiresUtc > DateTime.UtcNow && !string.IsNullOrEmpty(getRefreshTokenDetails.ProtectedTicket))
                {
                    if (getRefreshTokenDetails.DeviceType == refreshTokenModel.DeviceType)
                    {
                        var currentTime = DateTime.UtcNow;
                        Microsoft.Owin.Security.DataHandler.Serializer.TicketSerializer serializer = new Microsoft.Owin.Security.DataHandler.Serializer.TicketSerializer();
                        var getSecurityClaims = serializer.Deserialize(System.Text.Encoding.Default.GetBytes(getRefreshTokenDetails.ProtectedTicket));

                        //Generate New Refresh Token and Access Token
                        var tokenExpiration = Convert.ToDouble(ConfigurationManager.AppSettings["AccessTokenExpireTime"]);
                        var props = new AuthenticationProperties()
                        {
                            IssuedUtc = currentTime,
                            ExpiresUtc = DateTime.UtcNow.Add(TimeSpan.FromMinutes(tokenExpiration)),
                        };

                        var ticket = new AuthenticationTicket(getSecurityClaims.Identity, props);
                        var context = new Microsoft.Owin.Security.Infrastructure.AuthenticationTokenCreateContext(
                         Request.GetOwinContext(), Startup.OAuthOptions.AccessTokenFormat, ticket);
                        context.Ticket.Properties.Dictionary.Add(new KeyValuePair<string, string>("device_id", getRefreshTokenDetails.DeviceId));
                        var accessToken = Startup.OAuthOptions.AccessTokenFormat.Protect(ticket);
                        var refreshTokenId = Guid.NewGuid().ToString("n");
                        var refreshTokenLifeTime = Convert.ToDouble(ConfigurationManager.AppSettings["RefreshTokenExpireTime"]);


                        var refreshToken = new RefreshToken()
                        {
                            RefreshTokenId = GenerateHash.GetHash(refreshTokenId),
                            DeviceId = getRefreshTokenDetails.DeviceId,
                            DeviceType = refreshTokenModel.DeviceType,
                            UserId = getRefreshTokenDetails.UserId,
                            IssuedUtc = currentTime,
                            ExpiresUtc = currentTime.AddMinutes(Convert.ToDouble(refreshTokenLifeTime)),
                        };
                        context.Ticket.Properties.IssuedUtc = refreshToken.IssuedUtc;
                        context.Ticket.Properties.ExpiresUtc = refreshToken.ExpiresUtc;
                        refreshToken.ProtectedTicket = System.Text.Encoding.Default.GetString(serializer.Serialize(context.Ticket));

                        //SAVE Refresh token
                        refreshTokenRepository.SaveRefreshToken(refreshToken);

                        Dictionary<string, string> tokenResponse = new Dictionary<string, string>();
                        tokenResponse.Add("access_token", accessToken);
                        tokenResponse.Add("token_type", "bearer");
                        tokenResponse.Add("expires_in", TimeSpan.FromMinutes(tokenExpiration).TotalSeconds.ToString());
                        tokenResponse.Add("issued", ticket.Properties.IssuedUtc.Value.ToString("R"));
                        tokenResponse.Add("expires", ticket.Properties.ExpiresUtc.Value.ToString("R"));
                        tokenResponse.Add("refresh_token", refreshTokenId);
                        return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, tokenResponse));
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.Gone, apiResponse));
        }

        private Dictionary<string, string> GenerateTokenResponse(AppUser appUser, List<UserRoles> userRoles, string deviceId, DeviceType deviceType)
        {

            var tokenExpiration = Convert.ToDouble(ConfigurationManager.AppSettings["AccessTokenExpireTime"]);
            ClaimsIdentity identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, appUser.UserName));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()));
            identity.AddClaim(new Claim("displayName", appUser.Name));


            foreach (var userrole in userRoles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, userrole.RoleName));
            }
            var props = new AuthenticationProperties()
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(TimeSpan.FromMinutes(tokenExpiration)),
            };
            var ticket = new AuthenticationTicket(identity, props);
            var context = new Microsoft.Owin.Security.Infrastructure.AuthenticationTokenCreateContext(
             Request.GetOwinContext(), Startup.OAuthOptions.AccessTokenFormat, ticket);

            var accessToken = Startup.OAuthOptions.AccessTokenFormat.Protect(ticket);
            var refreshTokenId = Guid.NewGuid().ToString("n");
            var refreshTokenLifeTime = Convert.ToDouble(ConfigurationManager.AppSettings["RefreshTokenExpireTime"]);
            var refreshToken = new RefreshToken()
            {
                RefreshTokenId = GenerateHash.GetHash(refreshTokenId),
                UserId = appUser.Id,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(refreshTokenLifeTime),
                DeviceId = deviceId,
                DeviceType = deviceType
            };
            context.Ticket.Properties.IssuedUtc = refreshToken.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = refreshToken.ExpiresUtc;
            Microsoft.Owin.Security.DataHandler.Serializer.TicketSerializer serializer = new Microsoft.Owin.Security.DataHandler.Serializer.TicketSerializer();
            refreshToken.ProtectedTicket = System.Text.Encoding.Default.GetString(serializer.Serialize(context.Ticket));

            //Save new token
            refreshTokenRepository.SaveRefreshToken(refreshToken);

            Dictionary<string, string> tokenResponse = new Dictionary<string, string>();
            tokenResponse.Add("access_token", accessToken);
            tokenResponse.Add("token_type", "bearer");
            tokenResponse.Add("expires_in", TimeSpan.FromMinutes(tokenExpiration).TotalSeconds.ToString());
            tokenResponse.Add("issued", ticket.Properties.IssuedUtc.Value.ToString("R"));
            tokenResponse.Add("expires", ticket.Properties.ExpiresUtc.Value.ToString("R"));
            tokenResponse.Add("refresh_token", refreshTokenId);
            tokenResponse.Add("user_name", appUser.UserName);
            tokenResponse.Add("display_name", appUser.Name);
            return tokenResponse;
        }

    }
}
