using System;
using System.Net.Http;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using IspuWebGis.Net.Results;
using IspuWebGis.Net.Helpers;
using IspuWebGis.Net.Models.Authorization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace IspuWebGis.Net.Filters
{
    public class BasicAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public string Realm { get; set; }
        private const string TokenName = "token";
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;

            var userNameAndPasword = request.GetQueryNameValuePairs();
            string requestContent = request.Content.ReadAsStringAsync().Result;
          
            JObject contentObj = JObject.Parse(requestContent);
            string token = null;

            try
            {
                token = contentObj[TokenName].ToString();
            }
            catch (Exception e)
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing token in request or the format of token is invalid", request);
            }
            contentObj.Remove(TokenName);
            request.Content = new StringContent(JsonConvert.SerializeObject(contentObj), Encoding.UTF8,
                                    "application/json");

            if (token == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing token in request", request);
                return;
            }

            IPrincipal principal = GetPrincipal(token);

            if (principal == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid token", request);
            }
            else
            {
                // Authentication was attempted and succeeded. Set Principal to the authenticated user.
                context.Principal = principal;
            }
        }

        //TODO: Need to be changed
        private IPrincipal GetPrincipal(string token)
        {
            if(token != "token")
            {
                return null;
            }

            IIdentity identity = new Identity("IAmUser", "Token", true);
            IPrincipal principal = new Principal(identity, "User");

            return principal;
        }


        //******  Ненужные вещи, которые могут оказаться нужными потом =)
        private static Tuple<string, string> ExtractUserNameAndPassword(string authorizationParameter)
        {
            byte[] credentialBytes;

            try
            {
                credentialBytes = Convert.FromBase64String(authorizationParameter);
            }
            catch (FormatException)
            {
                return null;
            }

            // The currently approved HTTP 1.1 specification says characters here are ISO-8859-1.
            // However, the current draft updated specification for HTTP 1.1 indicates this encoding is infrequently
            // used in practice and defines behavior only for ASCII.
            Encoding encoding = Encoding.ASCII;
            // Make a writable copy of the encoding to enable setting a decoder fallback.
            encoding = (Encoding)encoding.Clone();
            // Fail on invalid bytes rather than silently replacing and continuing.
            encoding.DecoderFallback = DecoderFallback.ExceptionFallback;
            string decodedCredentials;

            try
            {
                decodedCredentials = encoding.GetString(credentialBytes);
            }
            catch (DecoderFallbackException)
            {
                return null;
            }

            if (String.IsNullOrEmpty(decodedCredentials))
            {
                return null;
            }

            int colonIndex = decodedCredentials.IndexOf(':');

            if (colonIndex == -1)
            {
                return null;
            }

            string userName = decodedCredentials.Substring(0, colonIndex);
            string password = decodedCredentials.Substring(colonIndex + 1);
            return new Tuple<string, string>(userName, password);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string parameter;

            if (String.IsNullOrEmpty(Realm))
            {
                parameter = null;
            }
            else
            {
                // A correct implementation should verify that Realm does not contain a quote character unless properly
                // escaped (precededed by a backslash that is not itself escaped).
                parameter = "realm=\"" + Realm + "\"";
            }

            context.ChallengeWith("Basic", parameter);
        }

        public bool AllowMultiple
        {
            get { return false; }
        }
    }
}