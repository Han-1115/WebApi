using System.IO;

using System.Text;
using System;
using System.Collections.Generic;

namespace BCS.Core.Utilities
{
    public static class DateTimeHelper
    {
        public static string FriendlyDate(DateTime? date)
        {
            if (!date.HasValue) return string.Empty;

            string strDate = date.Value.ToString("yyyy-MM-dd");
            string vDate = string.Empty;
            if (DateTime.Now.ToString("yyyy-MM-dd") == strDate)
            {
                vDate = "今天";
            }
            else if (DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") == strDate)
            {
                vDate = "明天";
            }
            else if (DateTime.Now.AddDays(2).ToString("yyyy-MM-dd") == strDate)
            {
                vDate = "后天";
            }
            else if (DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") == strDate)
            {
                vDate = "昨天";
            }
            else if (DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd") == strDate)
            {
                vDate = "前天";
            }
            else
            {
                vDate = strDate;
            }

            return vDate;
        }


        /// <summary>
        /// 根据月份 所有这个月的所有日期集合
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static List<string> GetAllDatesInMonth(int year, int month)
        {
            List<string> dates = new List<string>();

            int daysInMonth = DateTime.DaysInMonth(year, month);

            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime date = new DateTime(year, month, day);
                dates.Add(date.ToString("yyyy-MM-dd"));
            }

            return dates;
        }


        /// <summary>
        /// 返回当前月份从一号到今天的所有日期
        /// </summary>
        /// <returns></returns>
        public static List<string> GetDatesOfMonth()
        {
            List<string> datesOfMonth = new List<string>();
            for (int day = 1; day <= DateTime.Now.Day - 1; day++)
            {
                datesOfMonth.Add(new DateTime(DateTime.Now.Year, DateTime.Now.Month, day).ToString("yyyy-MM-dd"));
            }

            return datesOfMonth;
        }
    }
}

