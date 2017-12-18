using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IspuWebGis.Net.Models.Authorization
{
    public class AuthRequest
    {
        public AuthRequest(string name, string password)
        {
            this.name = name;
            this.password = password;
        }
        public string name { get; set; }
        public string password { get; set; }
    }
}