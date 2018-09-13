using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Standard.Infrastructure.Iocs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Infrastructure.Helpers
{
    public static class WebHelper
    {
        /// <summary>
        /// 初始化Web操作
        /// </summary>
        static WebHelper()
        {
            try
            {
                HttpContextAccessor = Ioc.GetService<IHttpContextAccessor>();
                Environment = Ioc.GetService<IHostingEnvironment>();
            }
            catch
            {
            }
        }
        /// <summary>
        /// Http上下文访问器
        /// </summary>
        public static IHttpContextAccessor HttpContextAccessor { get; set; }

        /// <summary>
        /// 当前Http上下文
        /// </summary>
        public static HttpContext HttpContext => HttpContextAccessor?.HttpContext;

        /// <summary>
        /// 宿主环境
        /// </summary>
        public static IHostingEnvironment Environment { get; set; }

        public static Dictionary<string, string> CreateDic(string url)
        {
            string[] strArray = url.Split(new Char[] { '&' });
            Dictionary<string, string> strs = new Dictionary<string, string>();
            string[] strArray1 = strArray;
            for (int i = 0; i < (int)strArray1.Length; i++)
            {
                string str = strArray1[i];
                string[] strArray2 = str.Split(new Char[] { '=' });
                if ((int)strArray2.Length == 2)
                {
                    if (!strs.ContainsKey(strArray2[0]))
                    {
                        strs.Add(strArray2[0], strArray2[1]);
                    }
                }
            }
            return strs;
        }
    }
}
