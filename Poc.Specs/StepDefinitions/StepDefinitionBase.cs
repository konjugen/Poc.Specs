using System;
using System.Collections.Generic;
using System.Linq;
using Specs.Utilities;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Specs.StepDefinitions
{
    [Binding]
    public class StepDefinitionBase : Base
    {
        [When(@"I go to url ""(.*)""")]
        [Given(@"I am on the ""(.*)""")]
        public void GivenIAmOnThe(string link)
        {
            Driver.Current.Navigate().GoToUrl(string.Format("{0}{1}", Constants.BaseUrl, link));
        }

        [Given(@"I am on the (.*) page")]
        public void GivenIAmOnThePage(string link)
        {
            Driver.Current.Navigate().GoToUrl(string.Format("{0}{1}", Constants.BaseUrl, link));
        }

        [Then(@"I click ""(.*)"" button")]
        public void ThenIClickButton(string linkText)
        {
            Driver.Current.FindElement(By.LinkText(linkText)).Click();
        }

        [When(@"The following element filled with data:")]
        [Given(@"The following element filled with data:")]
        public void GivenFilledData(Table table)
        {
            foreach (var row in table.Rows)
            {
                var index = 0;

                if (row.Keys.Contains("index") && string.IsNullOrEmpty(row["index"]))
                {
                    int.TryParse(row[index], out index);
                }

                if (row.Keys.Contains("isRequired") && row["isRequired"] == "false")
                {
                    try
                    {
                        HighlightElement(row["name"].GetElement(index));
                        row["name"].GetElement(index).Clear();
                        row["name"].GetElement(index).SendKeys(row["value"]);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
                else
                {
                    HighlightElement(row["name"].GetElement(index));
                    row["name"].GetElement(index).Clear();
                    row["name"].GetElement().SendKeys(row["value"]);
                }

            }
        }

        [When(@"The following element filled with random data:")]
        [Given(@"The following element filled with random data:")]
        public void GivenFilledRandomData(Table table)
        {
            foreach (var row in table.Rows)
            {
                var index = 0;

                if (row.Keys.Contains("index") && string.IsNullOrEmpty(row["index"]))
                {
                    int.TryParse(row[index], out index);
                }

                if (row.Keys.Contains("isRequired") && row["isRequired"] == "false")
                {
                    try
                    {
                        HighlightElement(row["name"].GetElement(index));
                        row["name"].GetElement(index).SendKeys(RandomString(5) + " " + RandomString(5));
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
                else
                {
                    HighlightElement(row["name"].GetElement(index));
                    row["name"].GetElement().SendKeys(RandomString(5) + " " + RandomString(5));
                }

            }
        }

        [When(@"I click ""(.*)""")]
        public void WhenIClick(string htmlText)
        {

            if (htmlText.GetElement() != null)
            {
                moveElement(htmlText);
                htmlText.GetElement().Click();
            }
            //Driver.Current.FindElement(By.CssSelector(htmlText)).Click();
        }

        [When(@"I click ""(.*)"" for facebook")]
        public void WhenIClickFB(string htmlText)
        {
            Driver.Current.FindElement(By.CssSelector(htmlText)).Click();

            //Facebook pop-up için ayrı bir method yazılmıştır
            //Bunun sebebi pop-up'daki butonların jQuery is not defined hatası vermesidir.
        }

        [Given(@"I click a ""(.*)""")]
        [When(@"I click a ""(.*)""")]
        public void WhenIClickA(string selector)
        {
            moveElement(selector);
            selector.GetElement().Click();
        }

        [When(@"I click a ""(.*)"" index ([0-9-]+)")]
        public void WhenIClickA(string selector, int index)
        {
            selector.GetElement(index).Click();
        }

        [When(@"I click a ""(.*)"" than new window will be open")]
        public void WhenIClickAThanNewWindowWillBeOpen(string selector)
        {
            var windowHandler = Driver.WindowFinder.Click(selector.GetElement());
            Driver.Current.SwitchTo().Window(windowHandler);
            Driver.CurrentWindowHandle = windowHandler;
        }

        [When(@"I return to main window")]
        public void WhenIReturnToMainWindow()
        {
            if (Driver.Current.WindowHandles.Contains(Driver.CurrentWindowHandle))
            {
                Driver.Current.Close();
            }

            Driver.Current.SwitchTo().Window(Driver.MainWindowHandle);
            Driver.CurrentWindowHandle = Driver.MainWindowHandle;
        }

        [When(@"I move mouse on the ""(.*)""")]
        public void WhenIMoveMouseOnThe(string selector)
        {
            Driver.Action.MoveToElement(selector.GetElement()).Perform();
        }

        [Then(@"I should see element ""(.*)""")]
        public void ThenIShouldSeeElement(string selector)
        {
            if (selector.GetElement() != null)
            {
                moveElement(selector);
                Assert.AreNotEqual(null, selector.GetElement());
            }
            else
            {
                Assert.Fail(selector + " element not found in the page.");
            }
        }

        [Then(@"I should see elements ""(.*)""")]
        public void ThenIShouldSeeElements(string selector)
        {
            for (int i = 1; i <= countWebElements(selector); i++)
            {
                string newelement = selector + ":nth-of-type(" + i + ")";
                moveElement(newelement);
                Assert.AreNotEqual(null, newelement.GetElement());
            }
        }

        [Then(@"Write value to log ""(.*)"" ""(.*)""")]
        public void ThenWriteValueToLog(string text, string selector)
        {
            Console.WriteLine(text, selector.GetElement().Text);
        }

        [Then(@"Write log ""(.*)"" ""(.*)""")]
        public void ThenWriteLog(string text, string variable)
        {
            Console.WriteLine(text, variable);
        }

        [Then(@"I should see element values")]
        public void ThenShouldElementValues(Table table)
        {
            foreach (var row in table.Rows)
            {
                Assert.AreEqual(row["value"], row["name"].GetElement().Text);
            }
        }

        [Then(@"I should see url")]
        public void ThenIShouldSeeUrl(Table table)
        {
            Assert.IsTrue(table.Rows.Select(x => Constants.BaseUrl + x["name"]).ToList().CheckUrl());
            Console.WriteLine(getUrl());
        }

        [Then(@"I should see url ""(.*)""")]
        public void ThenIShouldSeeUrl(string url)
        {
            Console.WriteLine(Constants.BaseUrl + url);
            Assert.IsTrue(new List<string> { Constants.BaseUrl + url }.CheckUrl());
        }

        [Then(@"I should see url include ""(.*)""")]
        public void ThenIShouldSeeUrlInclude(string url)
        {
            Console.WriteLine(getUrl());
            //Assert.That(getUrl(), Contains.Substring(url));
        }

        [StepArgumentTransformation(@"(.*)")]
        public List<string> StringToList(string list)
        {
            return list
                .Split(',')
                .Select(x => x.Trim())
                .ToList();
        }

        [When(@"Wait (.*) sec for page load")]
        [Then(@"Wait (.*) sec for page load")]
        public void WhenWaitSecForPageLoad(int p0)
        {
            System.Threading.Thread.Sleep(p0 * 1000);
        }

        [When(@"Fill ""(.*)"" textbox as ""(.*)""")]
        [Given(@"Fill ""(.*)"" textbox as ""(.*)""")]
        public void WhenFillTextboxAs(string htmlText, string text)
        {
            if (htmlText.GetElement() != null)
            {
                moveElement(htmlText);
                htmlText.GetElement().SendKeys(text);
            }
            //Driver.Current.FindElement(By.CssSelector(htmlText)).SendKeys(text);
        }

        [When(@"Fill ""(.*)"" textbox as ""(.*)"" for facebook")]
        public void WhenFillTextboxAsFB(string htmlText, string text)
        {
            Driver.Current.FindElement(By.CssSelector(htmlText)).SendKeys(text);

            //Facebook pop-up için ayrı bir method yazılmıştır
            //Bunun sebebi pop-up'daki textboxların jQuery is not defined hatası vermesidir.
        }

        [Then(@"I should see ""(.*)"" text is ""(.*)""")]
        public void ThenIShouldSeeTextIs(string htmlText, string text)
        {
            Console.WriteLine("Region is: " + getText(htmlText));
            Assert.AreEqual(getText(htmlText), text);
        }

        [When(@"I should focus element ""(.*)""")]
        public void WhenIShouldFocusElement(string htmlText)
        {
            moveElement(htmlText);
        }

        [Then(@"I see all links are working for ""(.*)""")]
        public void ThenISeeAllLinksAreWorkingFor(string htmlText)
        {
            checkBrokenLink(htmlText);
        }

        [Then(@"I see all images are working for ""(.*)""")]
        public void ThenISeeAllImagesAreWorkingFor(string htmlText)
        {
            checkBrokenIMG(htmlText);
        }

        [Then(@"I see all items are working for ""(.*)""")]
        public void ThenISeeAllItemsAreWorkingFor(string htmlText)
        {
            checkBrokenItems(htmlText);
        }

        [Given(@"I check ""(.*)""")]
        public void GivenICheck(string htmlText)
        {
            //Driver.Current.FindElement(By.CssSelector(htmlText)).Click;
        }


        [Given(@"Fill ""(.*)"" textbox as random mail")]
        [When(@"Fill ""(.*)"" textbox as random mail")]
        public void WhenFillTextboxAs(string htmlText)
        {
            moveElement(htmlText);
            htmlText.GetElement().SendKeys(RandomString(6) + "@" + RandomString(6) + ".com");
            //Driver.Current.FindElement(By.CssSelector(htmlText)).SendKeys(RandomString(6) + "@" + RandomString(6) + ".com");
        }

        [Given(@"Fill ""(.*)"" textbox as random number")]
        [When(@"Fill ""(.*)"" textbox as random number")]
        public void WhenFillTextboxAsNumber(string htmlText)
        {
            moveElement(htmlText);
            htmlText.GetElement().SendKeys(RandomInteger(11));
            //Driver.Current.FindElement(By.CssSelector(htmlText)).SendKeys(RandomInteger(11));
        }

        [When(@"I refresh page")]
        public void RefreshPage()
        {
            driver.Navigate().Refresh();
        }

    }
}
