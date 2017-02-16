using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using TechTalk.SpecFlow;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using OpenQA.Selenium.Interactions;
using Specs.Utilities;
using System.Net.Http;
using System.Net;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Specs.StepDefinitions
{
    public class Base
    {
        private const string CurrentPageKey = "Current.Page";
        public IWebDriver driver { get; set; }

        public Base()
        {
            driver = Driver.Current;
        }

        protected static String read(String value)
        {
            var data = new System.Collections.Generic.Dictionary<string, string>();
            foreach (var row in File.ReadAllLines(@"../../Resources/css.properties"))
                data.Add(row.Split('=')[0], string.Join("=", row.Split('=').Skip(1).ToArray()));
            return data[value];
        }

        private static Random random = new Random((int)DateTime.Now.Ticks);
        protected string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString().ToLower();
        }

        protected string RandomInteger(int size)
        {
            Random rnd = new Random();

            string ch = null;
            for (int i = 0; i < size; i++)
            {
                int number = rnd.Next(0, 9);
                ch = ch + number;
            }

            return ch;
        }

        protected void goToUrl(String Url)
        {
            driver.Navigate().GoToUrl(Url);
        }

        protected String getTitle()
        {
            return driver.Title;
        }

        protected String getUrl()
        {
            return driver.Url;
        }

        protected void waitForPageLoad(int second)
        {
            System.Threading.Thread.Sleep(second * 1000);
        }

        public void HighlightElement(IWebElement el)
        {
                IJavaScriptExecutor js = driver as IJavaScriptExecutor;
                js.ExecuteScript("arguments[0].setAttribute('style', arguments[1]);", el,
                        " border: 3px solid red;");
        }
        public void HighlightItem(string htmlText)
        {
            IWebElement el = htmlText.GetElement();
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("arguments[0].setAttribute('style', arguments[1]);", el,
                    " border: 3px solid red;");
        }

        protected IWebElement findWebElement(String htmlText)
        {
            IWebElement el = htmlText.GetElement();
            waitForPageLoad(1);
            return el;
        }

        protected int countWebElements(String htmlText)
        {
            //List<IWebElement> forms = driver.FindElements(By.CssSelector(htmlText)).ToList();
            var forms = htmlText.GetElements();
            int counter = forms.Count;
            return counter;
        }

        protected void clickWebElement(String htmlText)
        {
            if (htmlText.GetElement() != null)
            {
                IWebElement el = htmlText.GetElement();
                HighlightElement(el);
                el.Click();
            }
        }

        protected String getText(String htmlText)
        {
            IWebElement el = htmlText.GetElement();
            return el.Text;
        }

        protected String getInputValue(String htmlText)
        {
            IWebElement el = htmlText.GetElement();
            return el.GetAttribute("value");
        }

        protected Boolean isChecked(String htmlText)
        {
            IWebElement el = htmlText.GetElement(0, null, true);
            return el.Selected;
        }

        protected Boolean getWebElementIsDisplayed(String htmlText)
        {
            return findWebElement(htmlText).Displayed;
        }
        protected void fillTextBox(String htmlText, String value)
        {
            IWebElement el = htmlText.GetElement();
            HighlightElement(el);
            el.Clear();
            el.SendKeys(value);
        }

        protected void moveElement(String htmlText)
        {
            Actions actions = new Actions(driver);
            IWebElement element = htmlText.GetElement();
            HighlightElement(element);
            actions.MoveToElement(element);
            actions.Perform();
        }

        protected void moveToElement(IWebElement element)
        {
            Actions actions = new Actions(driver);
            HighlightElement(element);
            actions.MoveToElement(element);
            actions.Perform();
        }

        protected void checkBrokenItems(String htmlText)
        {
            //List<IWebElement> element = driver.FindElements(By.CssSelector(htmlText)).ToList();
            var element = htmlText.GetElements();
            Console.WriteLine("Element length: " + element.Count);

            for (int i = 0; i < element.Count; i++)
            {
                string code = null;
                try
                {
                    var client = new HttpClient();
                    var response = client.GetAsync(element[i].GetAttribute("href")).Result;
                    code = response.StatusCode.ToString();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Console.WriteLine(element[i].GetAttribute("href").ToString());
                        Console.WriteLine("Not Broken");
                        Console.WriteLine("Status is: " + code);
                    }
                    else
                    {
                        Console.WriteLine(element[i].GetAttribute("href").ToString());
                        Console.WriteLine("### Broken ###");
                        Console.WriteLine("Status is: " + code);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                Assert.AreEqual("OK", code);
            }

        }

        protected void checkBrokenLink(String htmlText)
        {
            //List<IWebElement> element = driver.FindElements(By.CssSelector(htmlText)).ToList();
            var element = htmlText.GetElements();
            Console.WriteLine("Element length: " + element.Count);

            for (int i = 0; i < element.Count; i++)
            {
                moveToElement(element[i]);
                string code = null;
                try
                {
                    var client = new HttpClient();
                    var response = client.GetAsync(element[i].GetAttribute("href")).Result;
                    code = response.StatusCode.ToString();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Console.WriteLine(element[i].GetAttribute("href").ToString());
                        Console.WriteLine("Not Broken");
                        Console.WriteLine("Status is: " + code);
                    }
                    else
                    {
                        Console.WriteLine(element[i].GetAttribute("href").ToString());
                        Console.WriteLine("### Broken ###");
                        Console.WriteLine("Status is: " + code);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                Assert.AreEqual("OK", code);
            }

        }

        protected void checkBrokenIMG(String htmlText)
        {
            //List<IWebElement> element = driver.FindElements(By.CssSelector(htmlText)).ToList();
            var element = htmlText.GetElements();
            Console.WriteLine("Element length: " + element.Count);

            for (int i = 0; i < element.Count; i++)
            {
                moveToElement(element[i]);
                string code = null;
                try
                {
                    var client = new HttpClient();
                    var response = client.GetAsync(element[i].GetAttribute("src")).Result;
                    code = response.StatusCode.ToString();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Console.WriteLine(element[i].GetAttribute("src").ToString());
                        Console.WriteLine("Not Broken");
                        Console.WriteLine("Status is: " + code);
                    }
                    else
                    {
                        Console.WriteLine(element[i].GetAttribute("src").ToString());
                        Console.WriteLine("### Broken ###");
                        Console.WriteLine("Status is: " + code);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                Assert.AreEqual("OK", code);
            }


        }

    }
}
