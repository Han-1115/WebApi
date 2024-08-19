using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Core.Utilities
{
    public static class UriHelper
    {

        /// <summary>
        /// 将对象的属性和属性值转换为URL键值对字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToQueryString<T>(T obj)
        {
            var properties = typeof(T).GetProperties();
            var keyValuePairs = new List<string>();
            foreach (var property in properties)
            {
                var key = property.Name;
                var value = Uri.EscapeDataString(property.GetValue(obj)?.ToString() ?? "");
                keyValuePairs.Add($"{key}={value}");
            }

            return string.Join("&", keyValuePairs);
        }
    }
}
