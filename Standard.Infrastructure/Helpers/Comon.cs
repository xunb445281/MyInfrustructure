using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Infrastructure.Helpers
{
    /// <summary>
    /// 常用公共操作
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// 获取类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        public static Type GetType<T>()
        {
            var type = typeof(T);
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        /// <summary>
        /// 换行符
        /// </summary>
        public static string Line => Environment.NewLine;

        /// <summary>
        /// 将byte[]转换未16进制字符串
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static string ConvertToHex(byte[] datas, bool isCaps = false)
        {
            StringBuilder result = new StringBuilder();

            string caps = "x2";

            if(!isCaps)
            {
                caps = "X2";
            }

            foreach(var data in datas)
            {
                result.Append(data.ToString(caps));
            }
            return result.ToString();
        }
    }
}
