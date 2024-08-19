using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using BCS.Core.Configuration;
using BCS.Core.Extensions;
using MimeKit;
using static System.Net.WebRequestMethods;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Identity.Client;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace BCS.Core.Utilities
{
    public static class MailHelper
    {
        private static string address { get; set; }
        private static string authPwd { get; set; }
        private static string name { get; set; }
        private static string host { get; set; }
        private static int port;
        private static bool enableSsl { get; set; }
        private static string clientId { get; set; }
        private static string tenantId { get; set; }

        static MailHelper()
        {
            IConfigurationSection section = AppSetting.GetSection("Mail");
            address = section["Address"];
            authPwd = section["AuthPwd"];
            name = section["Name"];
            host = section["Host"];
            port = section["Port"].GetInt();
            enableSsl = section["EnableSsl"].GetBool();

            IConfigurationSection azureAdsection = AppSetting.GetSection("AzureAd");
            clientId = azureAdsection["ClientId"];
            tenantId = azureAdsection["TenantId"];
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="subject">标题</param>
        /// <param name="body">内容</param>
        /// <param name="recipients">收件人</param>
        /// <param name="cc">抄送人</param>
        public static async Task<bool> Send(string subject, string body, string[] recipients, string[] cc = null)
        {
            try
            {
                var _sender = address;
                var _pasword = authPwd;  // The sender's Office 365 password. 

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_sender, _sender));

                foreach (var item in recipients)
                {
                    message.To.Add(new MailboxAddress(item, item));
                }

                if (cc != null)
                {
                    foreach (var item in cc)
                    {
                        message.Cc.Add(new MailboxAddress(item, item));
                    }
                }

                message.Subject = subject;

                var builder = new BodyBuilder
                {
                    HtmlBody = body
                };

                message.Body = builder.ToMessageBody();

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    var options = new PublicClientApplicationOptions
                    {
                        ClientId = clientId,  // Your client ID of the Azure App Registration. 
                        TenantId = tenantId   // Your Azure tenant ID. 
                    };

                    var pcab = PublicClientApplicationBuilder
                        .CreateWithApplicationOptions(options)
                        .Build();

                    var scopes = new[] { "email", "https://outlook.office365.com/SMTP.Send" };

#pragma warning disable CS0618 // Type or member is obsolete
                    var authToken = pcab.AcquireTokenByUsernamePassword(scopes, _sender,
                                        new NetworkCredential("", _pasword).SecurePassword)
                                        .ExecuteAsync();
#pragma warning restore CS0618 // Type or member is obsolete

                    await client.ConnectAsync(host, port,
                        SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(new SaslMechanismOAuth2(_sender,
                        authToken.Result.AccessToken));
                    await client.SendAsync(message);

                    await client.DisconnectAsync(true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }

}
