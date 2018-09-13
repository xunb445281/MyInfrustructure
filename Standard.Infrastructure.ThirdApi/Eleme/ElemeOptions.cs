using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Infrastructure.ThirdApi.Eleme
{
    public class ElemeOptions
    {
        public static string Key { get; set; }
        public static string Secret { get; set; }
        public static bool IsTest { get; set; }

        public static string GetAuthUrl()
        {
            if(IsTest)
            {
                return "https://open-api-sandbox.shop.ele.me/authorize";
            }
            else
            {
                return "https://open-api.shop.ele.me/authorize";
            }
        }

        public static string GetAuthTokenUrl()
        {
            if (IsTest)
            {
                return "https://open-api-sandbox.shop.ele.me/token";
            }
            else
            {
                return "https://open-api.shop.ele.me/token";
            }
        }

        public static string GetUrl()
        {
            if (IsTest)
            {
                return "https://open-api-sandbox.shop.ele.me/api/v1/";
            }
            else
            {
                return "https://open-api.shop.ele.me/api/v1/";
            }
        }
        
    }
}
