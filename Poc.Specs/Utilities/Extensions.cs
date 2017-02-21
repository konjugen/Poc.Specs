using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using System;
using System.Linq.Expressions;

namespace Specs.Utilities
{
    public static class Extensions
    {
        private static readonly int RetryCount = int.Parse(ConfigurationManager.AppSettings["timeout:for:second"]);

        //public static IWebElement GetElement(this string selector, int index = 0, string extend = null, bool isHidden = false)
        //{
        //    int current = 0;

        //    while (true)
        //    {
        //        current++;

        //        var element = FindElement(selector, index, extend);

        //        if (current >= RetryCount)
        //        {
        //            return null;
        //        }

        //        if (element != null)
        //        {
        //            if (!isHidden && element.Displayed)
        //            {
        //                return element;
        //            }

        //            if (isHidden && !element.Displayed)
        //            {
        //                return element;
        //            }
        //        }

        //        Thread.Sleep(1000);
        //    }
        //}

        public static IWebElement GetElement(this string selector, int index = 0, string extend = null, bool isHidden = false)
        {
            int retryCount = 0;
            while (true)
            {
                retryCount++;

                WaitForAjax();

                var element = FindElement(selector, index, extend);
                if (element == null)
                {
                    if (retryCount < 2)
                    {
                        Thread.Sleep(3000);
                        continue;
                    }
                    return null;
                }

                //if (!isHidden && element.Displayed)
                //{
                //    Console.WriteLine("Bulduk");
                //    return element;
                //}

                //if (isHidden && !element.Displayed)
                //{
                //    return element;
                //}
                if (element.Displayed == true)
                {
                    return element;
                }
                else
                {
                    Thread.Sleep(3000);
                    return element;
                }
            }
        }

        public static IList<IWebElement> GetElements(this string selector, string extend = null)
        {
            int retryCount = 0;
            while (true)
            {
                retryCount++;

                WaitForAjax();

                var elements = FindElements(selector, extend);
                if (elements == null)
                {
                    if (retryCount < 2)
                    {
                        Thread.Sleep(3000);
                        continue;
                    }
                    return null;
                }

                if (elements.All(e => e.Displayed))
                {
                    return elements;
                }
                else
                {
                    Thread.Sleep(3000);
                    return elements;
                }
            }
        }

        private static IWebElement FindElement(string selector, int index, string extend)
        {
            var path = "return $('" + selector + "')" + extend + "[" + index + "]";

            var element = (IWebElement)((IJavaScriptExecutor)Driver.Current).ExecuteScript(path);
            return element;
        }

        private static IList<IWebElement> FindElements(string selector, string extend)
        {
            var path = "return $('" + selector + "')" + extend;

            var elements = (IList<IWebElement>)((IJavaScriptExecutor)Driver.Current).ExecuteScript(path);
            return elements;
        }

        public static bool CheckUrl(this List<string> selector)
        {
            int index = 0;

            while (true)
            {
                index++;

                var url = Driver.Current.Url;

                var result = selector.Any(x => x == url);

                if (index >= RetryCount && !result)
                {
                    return false;
                }

                Thread.Sleep(1000);

                return result;               
            }

        }

        public static string GetText(this string selector)
        {
            var element = Driver.Current.FindElement(By.LinkText(selector));
            return element != null ? element.Text : null;
        }

        private static void WaitForAjax()
        {
            while (true) // Handle timeout somewhere
            {
                var ajaxIsComplete = (bool)((IJavaScriptExecutor)Driver.Current).ExecuteScript("return jQuery.active == 0");
                if (ajaxIsComplete)
                    break;
                Thread.Sleep(100);
            }
        }

        public static IDictionary<string, string> GetPageData(this IWebDriver source, params HtmlElementDataSelector[] selectors)
        {
            var result = new Dictionary<string, string>();
            foreach (var htmlElementDataSelector in selectors)
            {
                var element = source.FindElement(By.CssSelector(htmlElementDataSelector.CssSelector));
                if (element == null)
                {
                    continue;
                }

                var attributeSelector = htmlElementDataSelector as HtmlElementAttributeDataSelector;
                if (attributeSelector != null)
                {
                    result.Add(attributeSelector.PageDataName, element.GetAttribute(attributeSelector.AttributeName).Trim());

                }
                else
                {
                    result.Add(htmlElementDataSelector.PageDataName, htmlElementDataSelector.ElementDataType == HtmlElementDataType.Value ? element.GetAttribute("value").Trim() : element.Text.Trim());
                }
            }

            return result;
        }

        public static IWebElement GetElementById(string text)
        {
            var element = Driver.Current.FindElement(By.Id(text));
            return element;
        }
    }
}
