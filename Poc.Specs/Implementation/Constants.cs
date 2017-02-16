using System.Configuration;

namespace Specs.Utilities
{
    public static class Constants
    {
        public static string BaseUrl = ConfigurationManager.AppSettings["url"];
        public static string getDriver = ConfigurationManager.AppSettings["browser"];
        public static string BaseApi = ConfigurationManager.AppSettings["api"];

        public static class AddressDeliveryType
        {
            public const string Same = "same";
            public const string Different = "different";
        }

        public static class LoginType
        {
            public const string User = "user";
            public const string Order = "order";
            public const string WithOutLogin = "withoutlogin";
        }

        public static class PaymentType
        {
            public const string MoneyTransfer = "money transfer";
            public const string Phone = "phone";
            public const string CreditCard = "credit card";
            public const string PayOnDoor = "pay on door";
            public const string PayOnDoorByCreditCard = "pay on door by credit card";
        }

    }
}
