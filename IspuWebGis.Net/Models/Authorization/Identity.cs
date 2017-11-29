using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace IspuWebGis.Net.Models.Authorization
{
    public class Identity : IIdentity
    {
        private string _name;
        private string _authenticationType;
        private bool _isAuthenticated;
        public string Name
        {
            get
            {
                return _name;
            }
        }
        public string AuthenticationType
        {
            get
            {
                return _name;
            }
        }
        public bool IsAuthenticated
        {
            get
            {
                return _isAuthenticated;
            }
        }
        public Identity(string name, string authenticationType, bool isAuthenticated)
        {
            _name = name;
            _authenticationType = authenticationType;
            _isAuthenticated = isAuthenticated;
        }
    }
}