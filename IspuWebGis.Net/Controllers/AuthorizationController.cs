using DAL.Models;
using DAL.Repos;
using IspuWebGis.Net.Models.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IspuWebGis.Net.Controllers
{
    [RoutePrefix("api/auth")]
    [EnableCors("*", "*", "*")]
    public class AuthorizationController : ApiController
    {
        [HttpPost]
        [Route("newuser")]
        public string CreateUser([FromBody]AuthRequest authRequest)//ClientPoint clientPoint)
        {
            try
            {
                return AuthorizationRepo.createUser(authRequest.name, authRequest.password);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        [HttpPost]
        [Route("authorize")]
        public AuthResponse AuthorizeUser([FromBody]AuthRequest authRequest)
        {
            try
            {
                var user = AuthorizationRepo.authorizeUser(authRequest.name, authRequest.password);
                AuthResponse authResponse;
                if(user != null)
                {
                    authResponse = new AuthResponse(user.Token, user.TokenCreationTime+ AuthorizationRepo.tokenExpTime);
                }
                else
                {
                    authResponse = new AuthResponse(null, DateTime.Now);
                }

                return authResponse;
            }
            catch (Exception e)
            {
                var authResponse = new AuthResponse(null, DateTime.Now);
                return authResponse;
            }
        }
        [HttpPost]
        [Route("bytoken")]
        public UserResponse AuthorizeByToken([FromBody]string token)
        {
            try
            {
                var user = AuthorizationRepo.authorizeByToken(token);

                return new UserResponse(user.Token, user.TokenCreationTime + AuthorizationRepo.tokenExpTime, user.UserName);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
