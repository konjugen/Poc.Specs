using System;
using System.Configuration;
using System.Net;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using TechTalk.SpecFlow;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace Specs.Utilities
{
    public static class Driver
    {
        public const string Key = "driver";
        public const string WindowFinderKey = "finder";
        public const string ActionKey = "actions";
        public static HttpStatusCode httpStatusCode;

        public static IWebDriver Current
        {

            get
            {
                if (!ScenarioContext.Current.ContainsKey(Key))
                {
                    IWebDriver driver = null;

                    if (Constants.getDriver == "firefox")
                    {
                        driver = new FirefoxDriver();
                    }
                    else if (Constants.getDriver == "ie")
                    {
                        driver = new InternetExplorerDriver();
                    }
                    else if (Constants.getDriver == "chrome")
                    {
                        driver = new ChromeDriver();
                    }

                    driver.Manage()
                        .Timeouts()
                        .ImplicitlyWait(
                            TimeSpan.FromSeconds(int.Parse(ConfigurationManager.AppSettings["timeout:for:second"])));

                    MainWindowHandle = CurrentWindowHandle = driver.CurrentWindowHandle;

                    ScenarioContext.Current[Key] = driver;
                }

                return ScenarioContext.Current[Key] as IWebDriver;
            }
        }

        private static DesiredCapabilities DesiredCapabilities()
        {
            throw new NotImplementedException();
        }

        public static PopupWindowFinder WindowFinder
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey(WindowFinderKey))
                {
                    ScenarioContext.Current[WindowFinderKey] = new PopupWindowFinder(Current);
                }

                return ScenarioContext.Current[WindowFinderKey] as PopupWindowFinder;
            }
        }

        public static Actions Action
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey(ActionKey))
                {
                    ScenarioContext.Current[ActionKey] = new Actions(Current);
                }

                return ScenarioContext.Current[ActionKey] as Actions;
            }
        }

        public static string MainWindowHandle { get; set; }

        public static string CurrentWindowHandle { get; set; }
    }
}
