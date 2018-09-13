using Newtonsoft.Json;
using Standard.Infrastructure.Encryptions;
using Standard.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Standard.Infrastructure.ThirdApi.Eleme
{
    /// <summary>
    /// 饿了么请求参数
    /// </summary>
    public class ElmRequest
    {
        IDictionary<string, object> dicParams;

        public ElmRequest()
        {
            id = Guid.NewGuid().ToString();
            nop = "1.0.0";
            Params = new ExpandoObject();
            dicParams = Params as IDictionary<string, object>;
            metas = new ExpandoObject();
            metas.app_key = ElemeOptions.Key; //"wYO4C8ZLzB";  //ElmConfig.Current.ClientId;
            metas.timestamp = DateTimeHelper.CreateTimestamp(DateTimeType.Seconds); //1486217703;
            Secret = ElemeOptions.Secret; // "8e81ef8b73593d6b24e0cd9f11cbe3132d11ab8e";
        }

        public string id { get; set; }
        public string nop { get; set; }
        public string token { get; set; }
        public dynamic metas { get; set; }
        public string action { get; set; }
        public string signature { get; set; }
        [JsonProperty("params")]
        public dynamic Params { get; set; }
        [JsonIgnore]
        public string Secret { get; set; }

        /// <summary>
        /// 设置params中的属性
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetParams(string key, object value)
        {
            //var dic = (Params as IDictionary<string, object>);
            if (dicParams.ContainsKey(key))
            {
                dicParams[key] = value;
            }
            else
            {
                dicParams.Add(key, value);
            }
            //(Params as IDictionary<string, object>).Add(key, value);
        }

        /// <summary>
        /// 设置cleintkey和secret
        /// </summary>
        /// <param name="value"></param>
        public void SetAppKey(string appkey, string secret)
        {
            metas.app_key = appkey;
            this.Secret = secret;
        }

        /// <summary>
        /// 判断params中的属性是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ExistsParams(string key)
        {
            dicParams.TryGetValue(key, out object value);

            if (value == null)
                return false;

            if (string.IsNullOrWhiteSpace(value.ToString()))
                return false;

            return true;
        }

        /// <summary>
        /// 设置sign
        /// </summary>
        public void SetSign()
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new Exception("请求失败，token为空");
            }

            if (string.IsNullOrWhiteSpace(metas.app_key))
            {
                throw new Exception("请求失败，app_key为空");
            }

            string str = GetUnSign();

            var sign = Encrypt.Md5By32(str).ToUpper();

            signature = sign;
        }

        /// <summary>
        /// 生成未加密sign
        /// </summary>
        /// <returns></returns>
        private string GetUnSign()
        {
            SortedDictionary<string, object> dic = new SortedDictionary<string, object>();

            foreach (var item in metas)
            {
                dic.Add(item.Key, item.Value);
            }

            foreach (var item in Params)
            {
                dic.Add(item.Key, item.Value);
            }

            StringBuilder sb = new StringBuilder();

            foreach (var item in dic)
            {
                if (item.Value is string)
                {
                    sb.Append($"{item.Key}=\"{item.Value}\"");
                }
                else if (item.Value.GetType().IsValueType)
                {
                    sb.Append($"{item.Key}={item.Value}");
                }
                else
                {
                    sb.Append($"{item.Key}={JsonHelper.ToJson(item.Value)}");
                }

            }

            string str = action + token + sb.ToString() + Secret;

            return str;
        }
    }
}
