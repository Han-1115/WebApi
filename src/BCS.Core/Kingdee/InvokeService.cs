using BCS.Core.Configuration;
using BCS.Core.Utilities;
using BCS.Entity.DomainModels;
using BCS.Entity.DTO.Staff;
using Jint;
using Jint.Native;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BCS.Core.Kingdee
{
    public static class InvokeService
    {
        private static string userName { get; set; }

        private static string domain { get; set; }

        private static string otp { get; set; }

        static InvokeService()
        {
            IConfigurationSection kingdeeSection = AppSetting.GetSection("Kingdee");
            userName = kingdeeSection["UserName"];
            domain = kingdeeSection["Domain"];
            otp = kingdeeSection["OTP"];
        }

        /// <summary>
        /// 组织取数服务
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string InteOAGetAdminOrgDataService()
        {
            string result = string.Empty;
            try
            {
                string token = Token.CreateToken(userName, otp);
                Console.WriteLine(token);
                string serviceName = "inteOAGetAdminOrgDataService";
                HttpClient client = new HttpClient();
                Authorize.LoginsHR(client, domain, userName, otp);
                result = client.Get($"{domain}shr/shr/msf/service.do?method=callService&serviceName={serviceName}", "application/x-www-form-urlencoded", false);
            }
            catch (Exception ex)
            {
                throw new Exception("error" + ex.Message.ToString());
            }

            return result;
        }

        /// <summary>
        /// 职位取数服务
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string InteOAGetPositionDataService()
        {
            string result = string.Empty;
            try
            {
                string token = Token.CreateToken(userName, otp);
                Console.WriteLine(token);
                string serviceName = "inteOAGetPositionDataService";
                HttpClient client = new HttpClient();
                Authorize.LoginsHR(client, domain, userName, otp);
                result = client.Get($"{domain}shr/shr/msf/service.do?method=callService&serviceName={serviceName}", "application/x-www-form-urlencoded", false);
            }
            catch (Exception ex)
            {
                throw new Exception("error" + ex.Message.ToString());
            }

            return result;
        }

        /// <summary>
        /// 员工取数服务（启用）
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string InteOAGetPersonDataService()
        {
            string result = string.Empty;
            try
            {
                string token = Token.CreateToken(userName, otp);
                Console.WriteLine(token);
                string serviceName = "inteOAGetPersonDataService";
                HttpClient client = new HttpClient();
                Authorize.LoginsHR(client, domain, userName, otp);
                result = client.Get($"{domain}shr/shr/msf/service.do?method=callService&serviceName={serviceName}&filterType=1", "application/x-www-form-urlencoded", false);
            }
            catch (Exception ex)
            {
                throw new Exception("error" + ex.Message.ToString());
            }

            return result;
        }

        /// <summary>
        /// 员工取数服务（禁用）
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string InteOAGetLeavePersonDataService()
        {
            string result = string.Empty;
            try
            {
                string token = Token.CreateToken(userName, otp);
                Console.WriteLine(token);
                string serviceName = "inteOAGetPersonDataService";
                HttpClient client = new HttpClient();
                Authorize.LoginsHR(client, domain, userName, otp);
                result = client.Get($"{domain}shr/shr/msf/service.do?method=callService&serviceName={serviceName}&filterType=0", "application/x-www-form-urlencoded", false);
            }
            catch (Exception ex)
            {
                throw new Exception("error" + ex.Message.ToString());
            }

            return result;
        }

        /// <summary>
        /// 员工任职取数服务
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string InteOAGetEmpOrgRelationService()
        {
            string result = string.Empty;
            try
            {
                string token = Token.CreateToken(userName, otp);
                Console.WriteLine(token);
                string serviceName = "inteOAGetEmpOrgRelationService";
                HttpClient client = new HttpClient();
                Authorize.LoginsHR(client, domain, userName, otp);
                result = client.Get($"{domain}shr/shr/msf/service.do?method=callService&serviceName={serviceName}", "application/x-www-form-urlencoded", false);
            }
            catch (Exception ex)
            {
                throw new Exception("error" + ex.Message.ToString());
            }

            return result;
        }


        /// <summary>
        /// 获取考勤项目属性信息服务
        /// </summary>
        public static string GetAttendanceProjectService()
        {
            string result = string.Empty;
            try
            {
                string token = Token.CreateToken(userName, otp);
                Console.WriteLine(token);
                string serviceName = "getAttendanceProjectService";
                HttpClient client = new HttpClient();
                Authorize.LoginsHR(client, domain, userName, otp);
                result = client.Get($"{domain}shr/shr/msf/service.do?method=callService&serviceName={serviceName}", "application/x-www-form-urlencoded", false);
            }
            catch (Exception ex)
            {
                throw new Exception("error" + ex.Message.ToString());
            }

            return result;
        }


        /// <summary>
        /// 模拟浏览器xhr请求获取考勤数据
        /// </summary>
        /// <returns></returns>
        public static string GetAttendanceByBrowserXHR(string startDate, string endDate)
        {
            try
            {
                AttendanceOptions options = new AttendanceOptions();
                options._search = false;
                options.nd = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString(); //"1708244369901";
                options.rows = 50000;
                options.page = 1;
                options.sidx = string.Empty;
                options.sord = "asc";
                options.queryUuid = Guid.NewGuid().ToString();
                options.componentID = "grid";
                options.filterItems = "{\"infoSetScheme\":\"010ATS\"}";
                options.fastFilterItems = "{\"hrOrg\":{\"values\":\"\",\"isSaveToScheme\":true,\"dataType\":\"String\",\"url\":\"/dynamic.do?method=getHrOrgUnitData\",\"isSealUp\":false},\"adminOrgRange\":{\"values\":\"\",\"isSaveToScheme\":true,\"dataType\":\"String\",\"url\":\"/dynamic.do?uipk=com.kingdee.eas.hr.ats.app.HolidayLimit.list&method=getDefaultManager\"},\"adminOrgUnit\":{\"values\":\"\",\"isSaveToScheme\":true,\"dataType\":\"F7\",\"uipk\":\"com.kingdee.eas.basedata.org.app.AdminOrgUnit.F7\",\"includeSub\":\"0\",\"isIncludeSub\":true,\"dataItem\":[]},\"dateSet.date\":{\"values\":{\"startDate\":\"2024-02-01\",\"endDate\":\"2024-02-29\",\"selectDate\":\"thisMonth\"},\"isSaveToScheme\":true,\"dataType\":\"Date\",\"dataItem\":\"thisMonth\",\"queryDateType\":\"\"}}";
                // Parse JSON string to JObject
                JObject jsonObject = JObject.Parse(options.fastFilterItems);

                // Replace startDate and endDate with variables
                jsonObject["dateSet.date"]["values"]["startDate"] = startDate;
                jsonObject["dateSet.date"]["values"]["endDate"] = endDate;

                // Convert JObject back to string
                options.fastFilterItems = jsonObject.ToString();
                options.sorterItems = "person.id,dateSet.date asc";
                options.custom_params = "{\"debug\":\"true\",\"uipk\":\"com.kingdee.eas.hr.ats.app.AttendanceResultDetail.dynamicList\",\"inFrame\":\"true\",\"serviceId\":\"AeHKBR5wQZKXH6759hJi0fI9KRA=\"}";
                options.keyField = "ATS_RESULT.id";

                options.columnModel = string.Join(",", PropertyKeyMap.Map.Values);
                options.uipk = "com.kingdee.eas.hr.ats.app.AttendanceResultDetail.dynamicList";

                string param = UriHelper.ToQueryString<AttendanceOptions>(options);
                string token = Token.CreateToken(userName, otp);
                Console.WriteLine(token);
                HttpClient client = new HttpClient();
                Authorize.LoginsHR(client, domain, userName, otp);
                string result = client.Post($"{domain}/shr/dynamic.do?method=getListData&{param}", "application/x-www-form-urlencoded", "");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("error" + ex.Message.ToString());
            }
        }
    }
}
