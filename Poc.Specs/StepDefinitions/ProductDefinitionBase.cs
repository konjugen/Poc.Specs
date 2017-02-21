using System;
using System.Collections.Generic;
using Specs.Utilities;
using TechTalk.SpecFlow;

namespace Poc.Specs.StepDefinitions
{
    [Binding]
    public class ProductDefinition : Steps
    {
        [When(@"I add following products to cart ""(.*)""")]
        [Given(@"The following products were in cart ""(.*)""")]
        public void AddProductToCart(List<string> products)
        {
            Driver.Current.Navigate().GoToUrl(Constants.BaseUrl);

            foreach (var row in products)
            {
                Driver.Current.Navigate().GoToUrl(Constants.BaseUrl + row);

                //add to cart
                var submitButton = Extensions.GetElementById("productVariantAddToCart");
                if (submitButton != null)
                {
                    submitButton.Click();
                }
                else
                {
                    Console.WriteLine("Product \"{0}\" is not avail. Add to cart didn't work.", row);
                    continue;
                }

                var urlResult = (new List<string> { Constants.BaseUrl + "/Cart"}).CheckUrl();

                if (urlResult)
                {
                    continue;
                }

                var result = ".product__message-text".GetElement();
                if (result != null)
                {
                    Console.WriteLine("Product \"{0}\" is not avail. Reason : {1}", row, result.Text);
                }
            }
        }
    }
}
