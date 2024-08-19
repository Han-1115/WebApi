using BCS.Core.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCS.Core.Kingdee
{
    public static class Authorize
    {
        public static void LoginsHR(HttpClient client, string url, string userName, string otp)
        {           
            HttpClientDelegate callResult = new HttpClientDelegate(ResultCall);
            client.Url = url + string.Format("shr/OTP2sso.jsp?username={0}&userAuthPattern=otp&password={1}", userName, Token.CreateToken(userName, otp));            
            client.CookieContainer = new System.Net.CookieContainer();
            client.SysncRequest(DateTime.Now, callResult, "", false);
        }

        private static void ResultCall(DateTime sender, string content)
        {
            // Method intentionally left empty.
        }
    }
}
