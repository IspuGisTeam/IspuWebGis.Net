using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace IspuWebGis.Net.Models.Authorization
{
    public class Principal: IPrincipal
    {
        private IIdentity _identity;
        private string _role;

        public IIdentity Identity
        {
            get
            {
                return _identity;
            }
        }

        public bool IsInRole(string role)
        {
            return _role == role;
        }

        public Principal(IIdentity identity, string role)
        {
            _identity = identity;
            _role = role;
        }
    }
}