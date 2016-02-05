using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopifySharp;

namespace ShopifyCmd
{
    
    /// <summary>
    /// A utility class for running tests.
    /// </summary>
    public static class AppSettings
    {

        public static string ApiKey { get; set; }

        public static string SecretKey { get; set; }

        public static string AccessToken { get; set; }

        public static string MyShopifyUrl { get; set; }

        /// <summary>
        /// An access token to a shop created by a real application. This is only used for testing <see cref="ShopifyRecurringChargeService"/>,
        /// because a private app cannot create/manipulate charges.
        /// </summary>
        public static string BillingAccessToken { get; } = ConfigurationManager.AppSettings.Get("BillingAccessToken");

        /// <summary>
        /// A *.myshopify.com domain corresponding to <see cref="BillingAccessToken"/>.
        /// </summary>
        public static string BillingMyShopifyUrl { get; } = ConfigurationManager.AppSettings.Get("BillingMyShopifyUrl");

        static AppSettings()
        {
            ApiKey = ConfigurationManager.AppSettings.Get("ApiKey");
            AccessToken = ConfigurationManager.AppSettings.Get("AccessToken");
            MyShopifyUrl = ConfigurationManager.AppSettings.Get("MyShopifyUrl");
            SecretKey= ConfigurationManager.AppSettings.Get("SecretKey");

        }


    }
}
