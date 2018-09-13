using Standard.Infrastructure.Encryptions;
using Standard.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Infrastructure.ThirdApi.Meituan
{

    /// <summary>
    /// 美团参数
    /// </summary>
    public class MeituanRequest
    {

        /// <summary>
        /// 参数字典
        /// </summary>
        private SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();


        public MeituanRequest()
        {
            sortedDictionary.Add("charset", "utf-8");
            sortedDictionary.Add("timestamp", DateTimeHelper.CreateTimestamp());
        }


        public IDictionary<string, string> Params
        {
            get
            {
                return sortedDictionary;
            }
        }


        /// <summary>
        /// 判断token
        /// </summary>
        public bool ValidToken()
        {
            if (!ExistsKey("appAuthToken"))
            {
                throw new Exception("美团api接口，appAuthToken是必须的！");
            }
            return true;
        }


        /// <summary>
        /// 设置token
        /// </summary>
        /// <param name="token"></param>
        public void SetToken(string token)
        {
            sortedDictionary.Add("appAuthToken", token);
        }

        /// <summary>
        /// 设置sign
        /// </summary>
        public void SetSign()
        {
            ValidToken();
            string signkey = "123456789";
            string sing = MakeSigin(signkey);
            sortedDictionary.Add("sign", sing);
        }

        /// <summary>
        /// 设置key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetValue(string key, string value)
        {
            sortedDictionary.Add(key, value);
        }

        /// <summary>
        /// 获取value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetValue(string key)
        {
            sortedDictionary.TryGetValue(key, out string value);
            return value;
        }

        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool RemoveKey(string key)
        {
            return sortedDictionary.Remove(key);
        }

        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ExistsKey(string key)
        {
            return GetValue(key) != null;
        }

        /// <summary>
        /// 生成url参数
        /// </summary>
        /// <returns></returns>
        public string ToUrlParamter()
        {
            StringBuilder result = new StringBuilder();

            foreach (var item in sortedDictionary)
            {
                result.Append($"{item.Key}={item.Value}&");
            }

            if (result.Length > 0)
            {
                result.Remove(result.Length - 1, 1);
            }

            return result.ToString();
        }

        /// <summary>
        /// 生成sign
        /// </summary>
        /// <returns></returns>
        private string MakeSigin(string signkey)
        {
            string result = ToUrlParamter().Replace("&", "").Replace("=", ""); ;
            result = signkey + result;
            //var data = Encrypt.CreateSha1(result);
            var sign = Encrypt.CreateSha1ByLower(result).ToUpper(); //Common.ConvertToHex(data);
            return sign;
        }

    }
}
