using Standard.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
namespace Standard.Infrastructure.WebClients
{
    public class HttpWebClient
    {
        HttpClient client = null;

        public HttpWebClient()
        {
            client = new HttpClient();
        }

        /// <summary>
        /// 添加授权信息
        /// </summary>
        /// <param name="scheme"></param>
        /// <param name="parameter"></param>
        public void AddAuthorization(string scheme,string parameter)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, parameter);
        }
        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<HttpResponseData> PostAsync(string url, HttpContent content)
        {
            var httpMessage = await client.PostAsync(url, content);
            var result = await httpMessage.Content.ReadAsStringAsync();
            var res = new HttpResponseData();
            res.StatusCode = res.StatusCode;
            res.Result = result;
            res.Message = httpMessage.ReasonPhrase;
            return res;
        }
        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<HttpResponseData> GetAsync(string url)
        {
            var httpMessage = await client.GetAsync(url);
            var result = await httpMessage.Content.ReadAsStringAsync();
            var res = new HttpResponseData();
            res.StatusCode = res.StatusCode;
            res.Result = result;
            res.Message = httpMessage.ReasonPhrase;
            return res;
        }
        /// <summary>
        /// application/json请求
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<HttpResponseData> PostJsonAsync(HttpRequestData data)
        {
            string json = JsonHelper.ToJson(data.Data);
            StringContent content = new StringContent(json);
            content.Headers.ContentType = new
            System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            foreach (var item in data.Headers)
            {
                content.Headers.Add(item.Key, item.Value);
            }
            return await PostAsync(data.Url, content);
        }

        /// <summary>
        /// application/json请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<HttpResponseData> PostJsonAsync(string url,dynamic data)
        {
            HttpRequestData requestData = new HttpRequestData();
            requestData.Url = url;
            requestData.Data = data;
            return await PostJsonAsync(requestData);
        }

        /// <summary>
        /// application/x-www-form-urlencoded请求
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<HttpResponseData> PostFormAsync(HttpRequestData data)
        {
            FormUrlEncodedContent content = new
            FormUrlEncodedContent(data.FormData);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            foreach (var item in data.Headers)
            {
                content.Headers.Add(item.Key, item.Value);
            }
            return await PostAsync(data.Url, content);
        }
        /// <summary>
        /// application/x-www-form-urlencoded请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<HttpResponseData> PostFormAsync(string url, IDictionary<string, string> param)
        {
            HttpRequestData data = new HttpRequestData();
            data.Url = url;
            data.FormData = param;
            return await PostFormAsync(data);
        }
    }

    /// <summary>
    /// web请求返回实体
    /// </summary>
    public class HttpResponseData
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Result { get; set; }
        public string Message { get; set; }

        public string ErrorMessage
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if(!string.IsNullOrWhiteSpace(Result))
                {
                    sb.AppendLine(Result);
                }
                if (!string.IsNullOrWhiteSpace(Message))
                {
                    sb.AppendLine(Message);
                }
                return sb.ToString();
            }
        }
    }

    /// <summary>
    /// web请求实体
    /// </summary>
    public class HttpRequestData
    {
        public string Url { get; set; }
        private Dictionary<string, IEnumerable<string>> dictionary = new
        Dictionary<string, IEnumerable<string>>();
        public void Add(string name, string value)
        {
            Add(name, new List<string>() { value });
        }
        public void Add(string name, IEnumerable<string> values)
        {
            if (!dictionary.ContainsKey(name))
            {
                dictionary.Add(name, values);
            }
            else
            {
                dictionary[name] = values;
            }
        }
        public IDictionary<string, IEnumerable<string>> Headers
        {
            get
            {
                return this.dictionary;
            }
        }
        public IDictionary<string, string> FormData = new Dictionary<string,
        string>();
        public object Data { get; set; }
    }
}
