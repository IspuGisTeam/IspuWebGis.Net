using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IspuWebGis.Net.Models.Authorization
{
    public class UserResponse
    {
        public UserResponse(string token, DateTime tokenExpTime, string userName)
        {
            this.token = token;
            this.tokenExpTime = tokenExpTime;
            this.userName = userName;
        }
        public string token { get; set; }
        public DateTime tokenExpTime { get; set; }
        public string userName { get; set; }
    }
}