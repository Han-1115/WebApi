using BCS.Core.Utilities;
using BCS.Entity.DomainModels;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BCS.Core.Configuration;
using Microsoft.Extensions.Configuration;
using System.DrawingCore.Text;

namespace BCS.Core.Kingdee
{
    public static class Crawler
    {

        private static string userName { get; set; }

        private static string domain { get; set; }

        private static string otp { get; set; }

        static Crawler()
        {
            IConfigurationSection kingdeeSection = AppSetting.GetSection("Kingdee");
            userName = kingdeeSection["UserName"];
            domain = kingdeeSection["Domain"];
            otp = kingdeeSection["OTP"];
        }


        /// <summary>
        ///用爬虫爬取一段时间的考勤数据
        /// </summary>
        /// <returns></returns>
        public static List<StaffAttendance> GetSatffAttendanceByCrawler(string starDate, string endDate)
        {
            using (IWebDriver driver = new EdgeDriver())
            {
                driver.Manage().Window.Maximize();
                string url = $"{domain}/shr/dynamic.do?uipk=com.kingdee.eas.hr.ats.app.AttendanceResultDetail.dynamicList&serviceId=AeHKBR5wQZKXH6759hJi0fI9KRA%3D&inFrame=true";
                driver.Navigate().GoToUrl(url);
                Thread.Sleep(1000);

                var usernameElement = driver.FindElement(By.Id("username"));
                usernameElement.SendKeys(userName);
                var passwordElement = driver.FindElement(By.Id("password"));
                passwordElement.SendKeys(userName);

                var loginButton = driver.FindElement(By.Id("loginSubmit"));
                loginButton.Click();

                if (SeleniumHelper.IsElementPresent(driver, By.ClassName("dialogbox-btn")))
                {
                    var dialogboxBtn = driver.FindElement(By.ClassName("dialogbox-btn"));
                    dialogboxBtn.Click();
                }

                if (!string.IsNullOrWhiteSpace(starDate) && !string.IsNullOrWhiteSpace(endDate))
                {
                    var sartDateElement = driver.FindElement(By.Id("dateSet--date-datestart"));
                    sartDateElement.Clear();
                    sartDateElement.SendKeys(starDate);

                    var endDateElement = driver.FindElement(By.Id("dateSet--date-dateend"));
                    endDateElement.Clear();
                    endDateElement.SendKeys(endDate);
                }

                var serchBtn = driver.FindElement(By.Id("filter-search"));
                serchBtn.Click();
                Thread.Sleep(1000);

                var page = int.Parse(driver.FindElement(By.Id("sp_1_gridPager")).Text);
                List<StaffAttendance> satffAttendanceList = new List<StaffAttendance>();
                if (page > 0)
                {
                    for (int i = 1; i <= page; i++)
                    {
                        Thread.Sleep(1000);
                        IWebElement element = driver.FindElement(By.Id("sp_1_gridPager"));
                        IWebElement siblingElement = element.FindElement(By.XPath("preceding-sibling::*"));
                        siblingElement.Clear();
                        siblingElement.SendKeys(i.ToString());
                        siblingElement.SendKeys(Keys.Enter);
                        Thread.Sleep(1000);

                        var trs = driver.FindElements(By.ClassName("jqgrow")).ToList();
                        if (trs.Count > 0)
                        {
                            trs.ForEach(row =>
                            {
                                IList<IWebElement> tdElements = row.FindElements(By.TagName("td"));

                                if (tdElements.Count > 32)
                                {
                                    StaffAttendance satffAttendance = new StaffAttendance();
                                    Type type = typeof(StaffAttendance);
                                    var properties = type.GetProperties();
                                    if (properties != null && properties.Any())
                                    {
                                        for (int i = 1; i < properties.Length; i++)
                                        {
                                            int indexOfhtml = i + 1;
                                            var tdElement = tdElements[indexOfhtml];
                                            string value = string.Empty;
                                            if (SeleniumHelper.IsHasChildElement(tdElement, By.TagName("span")))
                                            {
                                                var spanElement = tdElement.FindElement(By.TagName("span"));
                                                value = spanElement.Text;
                                            }
                                            else
                                            {
                                                value = tdElement.Text;
                                            }

                                            var property = properties[i];
                                            if (property.PropertyType == typeof(decimal))
                                            {
                                                property.SetValue(satffAttendance, decimal.Parse(value));
                                            }
                                            else
                                            {
                                                property.SetValue(satffAttendance, value);
                                            }
                                        }
                                    }
                                    satffAttendanceList.Add(satffAttendance);
                                }
                            });
                        }
                    }
                }

                return satffAttendanceList;
            }
        }
    }
}
