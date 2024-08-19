using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Core.Utilities
{
    public static class SeleniumHelper
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);
        //判断元素是否存在
        public static bool IsElementPresent(IWebDriver driver, By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        //Wait for element loading based on custom conditions
        public static void WaitElementLoad(IWebDriver driver, Func<IWebDriver, bool> condition)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, DefaultTimeout);
                wait.Until(condition);
            }
            catch (WebDriverTimeoutException ex)
            {
                Console.WriteLine("Timeout waiting for elements to load inner text: " + ex.Message);
                throw ex;
            }
        }


        /// <summary>
        /// 判断元素是否含有子元素
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public static bool IsHasChildElement(IWebElement parentElement , By by)
        {
            try
            {
                parentElement.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;   
            }
        }


    }
}
