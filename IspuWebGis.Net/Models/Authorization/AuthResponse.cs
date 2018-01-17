using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IspuWebGis.Net.Models.Authorization
{
    public class AuthResponse
    {
        public AuthResponse(string token, DateTime tokenExpTime)
        {
            this.token = token;
            this.tokenExpTime = tokenExpTime;
        }
        public string token { get; set; }
        public DateTime tokenExpTime { get; set; }
    }
}