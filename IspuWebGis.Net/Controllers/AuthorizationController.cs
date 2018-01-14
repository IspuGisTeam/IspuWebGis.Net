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
    [RoutePrefix("api/newuser")]
    [EnableCors("*", "*", "*")]
    public class AuthorizationController : ApiController
    {
        [HttpPost]
        [Route("")]
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
    }
}
